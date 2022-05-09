using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;

namespace BPnn
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public MySqlConnection SQLconnection()
        {
            //连接数据库函数，这里用的是mysql            
            string connetStr = Class1.connetStr1;
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                Console.WriteLine("已连接数据库");
                richTextBox1.AppendText("已连接数据库\n");
                //输出时间
                string Time = Convert.ToString(DateTime.Now);
                Console.WriteLine(Time + "  " + "\n");
                richTextBox1.AppendText(Time + "  " + "\n");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("连接数据库失败\n" + ex.Message);
                richTextBox1.AppendText("连接数据库失败\n" + ex.Message);
                //输出时间
                string Time = Convert.ToString(DateTime.Now);
                Console.WriteLine(Time + "  " + "\n");
                richTextBox1.AppendText(Time + "  " + "\n");
            }
            return conn;
        }
        /**************参数*******************/
        public static int In = 3;    //输入层In个节点
        public static int hide = 7;  //隐藏层Hide个节点  习惯计算 2*In+1
        public static int Out = 1;   //输出层Out个节点
        public static int N = 146;    //训练使用N组数据
        public static int Ntest = 4;    //每次测试Ntest组数据
        public static double Err_max = 0.000001; //允许的误差精度
        public static double rate = 0.1; //学习速率
        public static double In_divisor = 1000; //输入除数 保持所有输入数据在-1~1内
        public static double Out_divisor = 1000; //输出除数 保持所有输出数据在-1~1内
        int Times = 100000; //训练次数
        public static int N1 = 20;  //训练集个数
        /******************************************/
        double[,] weight_IH = new double[In, hide];  //输入层隐藏层间权值矩阵 IH = Input -> Hide
        double[,] weight_HO = new double[hide, Out]; //隐藏层输出层间权值矩阵 OH = Output -> Hide
        double[] bias_H = new double[hide]; //隐藏层偏置
        double[] bias_O = new double[Out];  //输出层偏置
        double[,] delta_IH = new double[N, hide]; //输入层到隐藏层 修正值
        double[,] delta_HO = new double[N, Out];  //隐藏层到输出层 修正值
        double[] error = new double[N];    //一组样本误差
        double Err_Sum = 0; //一次训练总误差 及 每组样本误差和
        double[,] train_In = new double[N, In];   //训练输入值
        double[,] train_Out = new double[N, Out]; //训练输出值
        double[,] test_In = new double[N1, In];
        double[,] test_Out = new double[N1, Out];
        double[,] hide_Out = new double[N, hide]; //隐藏层输出
        double[,] BP_Out = new double[N, Out];  //BP网络最终输出
        //double[] err = new double[Times];
        ArrayList err = new ArrayList();
        public double random()
        {
            //产生随机数
            var seed = Guid.NewGuid().GetHashCode();
            Random r = new Random(seed);
            int i = r.Next(0, 100000);
            return (double)i / 100000;
        }
        public double sigmoid(double x)
        {
            //激活函数
            double tmp = 1.0 / (1.0 + Math.Exp(-x));
            return tmp;
        }
        public float getdata(string table, string col, int id, MySqlConnection conn)
        {
            //从数据库中取数据
            float ans;
            string sql1 = "select " + col + " from " + table + " where num = " + id;
            //string sql1 = "select " + item + " from " + table + " where id =" + id;
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            ans = float.Parse(cmd1.ExecuteScalar().ToString());
            //Console.WriteLine($"Predicted: {ans}\n");
            //richTextBox1.AppendText($"Predicted: {ans}\n");
            return ans;
        }
        //获取网络权重、偏置参数
        public void getweight(MySqlConnection conn)
        {
            int i = 0, j = 0;
            //weight_IH
            for (i = 0; i < In; i++)
            {
                for (j = 0; j < hide; j++)
                {
                    int a = j + 1;
                    string col = "c" + a;
                    weight_IH[i, j] = getdata("weight_in", col, i+1, conn);
                }
            }
            
            //weight_HO
            for (i = 0; i < hide; i++)
            {
                for (j = 0; j < Out; j++)
                {
                    int a = j + 1;
                    string col = "c" + a;
                    weight_HO[i, j] = getdata("weight_ho", col, i+1, conn);
                }
            }
            //bias_H 
            for (i = 0; i < hide; i++)
            {
                string col = "c1";
                bias_H[i] = getdata("bias_h", col, i+1, conn);
            }
            //bias_O
            for (i = 0; i < Out ; i++)
            {
                string col = "c1";
                bias_O[i] = getdata("bias_o", col, i+1, conn);
            }
           
        }
        public void insertdata(double d, int id, string col, string table, MySqlConnection conn)
        {
            string sql = "update " + table + " set " + col + " = " + d + " where num=" + id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine($"{col}成功写入数据库\n");
                //richTextBox1.AppendText("成功写入数据库\n");
            }
        }
        //将权重、偏置参数写入数据库
        public void writeweight(MySqlConnection conn)
        {
            int i = 0, j = 0;
            double d = 0;
            //weight_IH
            for (i = 0; i < In; i++)
            {
                for (j = 0; j < hide; j++)
                {
                    int a = j + 1;
                    string col = "c" + a;
                    d = weight_IH[i, j];
                    insertdata(d, i+1, col, "weight_in", conn);
                }
            }
            //weight_HO
            for (i = 0; i < hide; i++)
            {
                for (j = 0; j < Out; j++)
                {
                    int a = j + 1;
                    string col = "c" + a;
                    d = weight_HO[i, j];
                    insertdata(d, i+1, col, "weight_ho", conn);
                }
            }
            //bias_H 
            for (i = 0; i < hide; i++)
            {
                string col = "c1";
                d = bias_H[i];
                insertdata(d, i+1, col, "bias_h", conn);

            }
            //bias_O
            for (i = 0; i < Out; i++)
            {
                string col = "c1";
                d = bias_O[i];
                //= getdata("bias_o", col, i, conn);
                insertdata(d, i+1, col, "bias_h", conn);
            }
        }
        //获取训练数据 输入输出
        public void getTrainData( MySqlConnection conn)
        {
            int i = 0;//行                       

            for (i = 0; i <N; i++)
            {
                //301
                train_In[i, 0] = getdata("train", "s301",  i+1, conn);
                train_In[i, 0] /= In_divisor;       //保持所有输入数据在-1~1内 

                //303
                train_In[i, 1] = getdata("train", "s303",  i+1, conn);
                train_In[i, 1] /= In_divisor;       //保持所有输入数据在-1~1内 

                //305
                train_In[i, 2] = getdata("train", "s305",  i+1, conn);
                train_In[i, 2] /= In_divisor;       //保持所有输入数据在-1~1内 

                //厚度
                train_Out[i, 0] = getdata("train", "h",  i+1, conn);
                train_Out[i, 0] /= Out_divisor;      //保持所有输出数据在-1~1内 
               
            }

        }
        //获取测试数据集的数据
        //编号错了，懒得改，没影响
        public void getTestData(MySqlConnection conn)
        {
            int i = 0;//行
            int n = N1;
            int num = n;

            for (i = 0; i < n; i++)
            {
                //301
                test_In[i, 0] = getdata("test", "s301", 1+ i, conn);
                test_In[i, 0] /= In_divisor;       //保持所有输入数据在-1~1内 

                //302
                test_In[i, 1] = getdata("test", "s302", 1+ i, conn);
                test_In[i, 1] /= In_divisor;       //保持所有输入数据在-1~1内 

                //303
                test_In[i, 2] = getdata("test", "s302", 1 + i, conn);
                test_In[i, 2] /= In_divisor;       //保持所有输入数据在-1~1内 

                //厚度
                test_Out[i, 0] = getdata("test", "h", 1 + i, conn);
                test_Out[i, 0] /= Out_divisor;      //保持所有输出数据在-1~1内 

            }

        }

        //单个神经元计算
        public double getOneValue(int id, double[,] value, int c, double[,] weight, int num, double bias)
        {
            //输入值所在行、输入值、列数、权重、神经元编号 \偏置
            int i = 0; double tmp = 0.0;
            for (i = 0; i < c; i++)
            {
                tmp = tmp + value[id, i] * weight[ i,num];
            }
            tmp = tmp - bias;
            return sigmoid(tmp);
        }

        //训练times次
        public void train(int times)
        {
            int i, j, k, n;
            for (n = 0; n < times; n++)
            {
                Err_Sum = 0;
                double[] delta_err = new double[N];
                for (i = 0; i < N; i++)
                { delta_err[i] = 0; }

                //正向传输
                for (i = 0; i < N; i++)
                {
                    //计算隐藏层输出 in -> hide
                    for (j = 0; j < hide; j++)
                    {
                        hide_Out[i, j] = getOneValue(i, train_In, In, weight_IH, j, bias_H[j]);

                    }
                    //计算输出层输出 hide -> out
                    for (j = 0; j < Out; j++)
                    {
                        BP_Out[i, j] = getOneValue(i, hide_Out, hide, weight_HO, j, bias_O[j]);
                        //if ((times - n) == 1)
                         //   Console.WriteLine("最后一次："+BP_Out[i, j] * Out_divisor + "\r\n"); //最后一次训练打印每组输出
                          //  richTextBox1.AppendText("最后一次："+BP_Out[i, j] * Out_divisor + "\r\n"); //最后一次训练打印每组输出
                    }
                    //计算样本误差 
                    for (j = 0; j < Out; j++)
                    {
                        delta_err[i] = train_Out[i, j] - BP_Out[i, j]; //误差
                        error[i] += delta_err[i] * delta_err[i];  //误差平方和
                    }
                    error[i] /= 2;
                    //计算隐藏层输出层间权值调整系数  有公式
                    for (j = 0; j < Out; j++)
                    {
                        delta_HO[i, j] = delta_err[i] * BP_Out[i, j] * (1 - BP_Out[i, j]);
                    }
                    //计算输入层到隐藏层的权值调整系数  有公式
                    for (j = 0; j < hide; j++)
                    {
                        delta_IH[i, j] = 0; //清0
                        for (k = 0; k < Out; k++)
                        {
                            delta_IH[i, j] += delta_HO[i, k] * weight_HO[j, k] * hide_Out[i, j] * (1 - hide_Out[i, j]);
                        }
                    }
                    Err_Sum += error[i]; //计算
                   
                }
                err.Add(Err_Sum);
                //反向传输
                //调整weight_IH
                double temp = 0.0;
                for (i = 0; i < In; i++)
                {
                    for (j = 0; j < hide; j++)
                    {
                        temp = 0;
                        for (k = 0; k < N; k++)
                        {
                            temp += delta_IH[k, j] * train_In[k, i];
                        }
                        weight_IH[i, j] += rate * temp;

                    }
                }
                //调整bias_H
                for (i = 0; i < hide; i++)
                {
                    temp = 0;
                    for (j = 0; j < N; j++)
                    {
                        temp -= delta_IH[j, i];
                    }
                    bias_H[i] += rate * temp;
                }
                //调整weight_HO
                for (i = 0; i < hide; i++)
                {
                    for (j = 0; j < Out; j++)
                    {
                        temp = 0;
                        for (k = 0; k < N; k++)
                        {
                            temp += delta_HO[k, j] * hide_Out[k, i];
                        }
                        weight_HO[i, j] += rate * temp;
                    }
                }
                //调整bias_O
                for (i = 0; i < Out; i++)
                {
                    temp = 0;
                    for (j = 0; j < N; j++)
                    {
                        temp -= delta_HO[j, i];
                    }
                    bias_O[i] += rate * temp;
                }
                if (Err_Sum < Err_max) break; //误差到达允许值 跳出循环 停止训练
                 

                if ((n % (times / 10)) == 0)
                { //每Times/10次打印一次信息
                    Console.WriteLine("次数:" + (n + times / 10) + " \r误差:" + Err_Sum + "\r\n");
                    Console.WriteLine("----------------------------------------------------------\r\n");
                    richTextBox1.AppendText("次数:" + (n + times / 10) + " \r误差:" + Err_Sum + "\r\n");
                    richTextBox1.AppendText("----------------------------------------------------------\r\n");
                }
            }
        }
        //获取训练结果
        public double test(double[,] data)
        {
            double output = 0;
            int i;
            double[,] Hide = new double[1,hide];
            /*for (i = 0; i < N; i++)
            { Hide[0,i] = 0; }
            */
            for (i = 0; i < hide; i++)
            {
                Hide[0,i] = getOneValue(0, data, In, weight_IH, i, bias_H[i]);

            }
            for (i = 0; i < Out; i++)
            {
                output = getOneValue(0, Hide, hide, weight_HO, i, bias_O[i]);
            }

            return output;
        }
        //初始化神经网络
        public void parm_init()
        {
            int i, j;
            //srand((unsigned)time(NULL));    //撒种子生成随机数
            //初始化 输入层到隐藏层 权值
            for (i = 0; i < In; i++)
            {
                for (j = 0; j < hide; j++)
                {
                    weight_IH[i, j] = (double)(random() % 20 - 10) / 10; //取随机数-1.0 ~ +1.0
                }
            }
            //初始化 隐藏层 偏置  
            for (i = 0; i < hide; i++)
            {
                bias_H[i] = (double)(random() % 10) / 10; //取随机数0.0 ~ +1.0
            }
            //初始化 隐藏层到输出层 权值
            for (i = 0; i < hide; i++)
            {
                for (j = 0; j < Out; j++)
                {
                    weight_HO[i, j] = (double)(random() % 20 - 10) / 10; //取随机数-1.0 ~ +1.0
                }
            }
            //初始化 输出层 偏置 
            for (i = 0; i < Out; i++)
            {
                bias_O[i] = (double)(random() % 10) / 10; //取随机数0.0 ~ +1.0
            }
            for (i = 0; i < N; i++)
            {
                error[i] = 0; //初始化一组样本总误差 
                for (j = 0; j < hide; j++)
                {
                    delta_IH[i, j] = 0; //输入层到隐藏层 修正值
                }
                for (j = 0; j < Out; j++)
                {
                    delta_HO[i, j] = 0; //隐藏层到输出 修正值
                }
            }

        }

      



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            MySqlConnection conn = SQLconnection();
            //getweight(conn);
            parm_init();            
            getTrainData(conn);
            getTestData(conn);
                        
            int TIME = int.Parse(numericUpDown1.Value.ToString());
            richTextBox1.AppendText($"训练{TIME}轮\n");
            train(TIME );
            writeweight(conn);
            
            double[,] data = new double [1,3];
            double[] ans = new double[N1];
            for (int i=0;i<N1;i++)
            {
                for (int j=0;j<3;j++)
                {
                    data[0,j] = test_In[i,j];                    
                }
                 ans[i] = test(data)*1000;
                //输出结果                   
                richTextBox1.AppendText($"真实值{test_Out[i, 0]*1000}\n");
                richTextBox1.AppendText($"预测值{ans[i]}\n");
            }

            double[,] t = new double[N1, 1];
            for (int i=0;i<N1;i++)
            {
                t[i, 0] = test_Out[i, 0] * 1000;
            }


            int[] num = new int[N1];
            for (int i=0;i<N1;i++)
            {
                num[i] = i+1;
            }


            Series ss = new Series("预测");   //这里 dt1 ,dt2 任意取名称，但要唯一
            ss.Points.DataBindXY(num,ans);            
            ss.ChartType = SeriesChartType.Point;   //设置Y轴为折线
            chart1.Series.Add(ss);

            Series ss2 = new Series("实际");   //这里 dt1 ,dt2 任意取名称，但要唯一
            ss2.Points.DataBindXY(num, t);
            ss2.ChartType = SeriesChartType.Point;   //设置Y轴为折线
            chart1.Series.Add(ss2);
            chart1.Titles.Add("预测值与实际值的对比");
            chart1.ChartAreas[0].AxisX.Title = "编号";
            chart1.ChartAreas[0].AxisY.Title = "厚度";
            //训练loss


            int[] n = new int[TIME];
             for (int i = 0; i < TIME; i++)
             {
                 n[i] = i + 1;
             }

             Series ss3 = new Series("Loss");
             ss3.Points.DataBindXY(n,err);
             ss3.XValueType = ChartValueType.Int32; //设置X轴
             ss3.YValueType = ChartValueType.Double; //设置Y轴
             ss3.ChartType = SeriesChartType.Point;   //设置Y轴为折线
             chart2.Series.Add(ss3);
             chart2.Titles.Add("Loss");
             chart2.ChartAreas[0].AxisX.Title = "次数";
             chart2.ChartAreas[0].AxisY.Title = "loss";



            //测试getweight()
            // getweight(conn);                       
            //writeweight(conn);
            //getdata("train", "s301", i + 1, conn);

            //richTextBox1.AppendText($"{a}\n");

        }

        private void chart2_Click(object sender, EventArgs e)
        {
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 form1 = new Form1();
            form1.WindowState = FormWindowState.Maximized;
            form1.ShowDialog();
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
