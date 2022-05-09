using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
//连mysql数据库
using MySql.Data.MySqlClient;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string path=@"C:\Users\fair yang\Desktop\test.txt";
            System.IO.File.WriteAllLines(path, new string[] { "123456","098765" });*/


            //连接数据库，这里用的是mysql
            /* String connetStr = "server=127.0.0.1;port=3308;user=root;password=123456; database=test1;";
             MySqlConnection conn = new MySqlConnection(connetStr);
             try
             {
                 conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                 Console.WriteLine("已连接数据库");
                 richTextBox1.AppendText("已连接数据库\n");
                 //输出时间
                 string Time = Convert.ToString(DateTime.Now);
                 richTextBox1.AppendText(Time + "  " + "\n");
             }
             catch (MySqlException ex)
             {
                 Console.WriteLine(ex.Message);
                 richTextBox1.AppendText("连接数据库失败\n"+ex.Message);
             }*/

            /**************修改如下值*******************/
            int In = 2;    //输入层In个节点
            int Hide = 5;  //隐藏层Hide个节点
            int Out = 1;   //输出层Out个节点
            int N = 12;    //每次训练N组数据
            int Ntest= 4;    //每次测试Ntest组数据
            double Err_max = 0.000001; //允许的误差精度
            double rate =  0.1; //学习速率
            double In_divisor =  1.0; //输入除数 保持所有输入数据在-1~1内
            double Out_divisor =  4.0; //输出除数 保持所有输出数据在-1~1内
            int Times = 100000; //训练次数
            /******************************************/
            double [,] weight_IH=new double[In,Hide];  //输入层隐藏层间权值矩阵 IH = Input -> Hide
            double [,] weight_HO=new double[Hide,Out]; //隐藏层输出层间权值矩阵 OH = Output -> Hide
            double [] bias_H=new double[Hide]; //隐藏层偏置
            double [] bias_O=new double[Out];  //输出层偏置
            double [,] delta_IH=new double[N,Hide]; //输入层到隐藏层 修正值
            double [,] delta_HO= new double[N,Out];  //隐藏层到输出层 修正值

            double [] error=new double[N];    //一组样本误差
            double Err_Sum = 0; //一次训练总误差 及 每组样本误差和

            double [,] train_In=new double [N,In];   //训练输入值
            double [,] train_Out=new double [N,Out]; //训练输出值
            double [,] hide_Out=new double [N,Hide]; //隐藏层输出
            double [,] BP_Out=new double [N,Out];  //BP网络最终输出


           double random()
            {
                var seed = Guid.NewGuid().GetHashCode();
                Random r = new Random(seed);
                int i = r.Next(0, 100000);
                return (double)i / 100000;
            }

             void parm_init()
            {
                int i, j;
                //srand((unsigned)time(NULL));    //撒种子生成随机数
                                                  //初始化 输入层到隐藏层 权值
                for (i = 0; i < In; i++)
                {
                    for (j = 0; j < Hide; j++)
                    {
                        weight_IH[i,j] = (double)(random() % 20 - 10) / 10; //取随机数-1.0 ~ +1.0
                    }
                }
                //初始化 隐藏层 偏置  
                for (i = 0; i < Hide; i++)
                {
                    bias_H[i] = (double)(random() % 10) / 10; //取随机数0.0 ~ +1.0
                }
                //初始化 隐藏层到输出层 权值
                for (i = 0; i < Hide; i++)
                {
                    for (j = 0; j < Out; j++)
                    {
                        weight_HO[i,j] = (double)(random() % 20 - 10) / 10; //取随机数-1.0 ~ +1.0
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
                    for (j = 0; j < Hide; j++)
                    {
                        delta_IH[i,j] = 0; //输入层到隐藏层 修正值
                    }
                    for (j = 0; j < Out; j++)
                    {
                        delta_HO[i,j] = 0; //隐藏层到输出 修正值
                    }
                }

            }

            //激活函数
            double sigmoid(double x)
            {
                double tmp = 1.0 / (1.0 + Math.Exp(-x));
                return tmp;
            }
            //获取单节点输出 数据首地址 数据列数 权值首地址 权值列数 偏置
            double getOneNodeValue(double* value, int valueColumn, double* weight, int weightColumn, double bias)
            {
                int i = 0; double tmp = 0.0;
                for (i = 0; i < valueColumn; i++)
                    tmp += *(value + i) * *(weight + i * weightColumn); //x1*w1 + x2*w2 + x3*w3 + ....
                tmp -= bias; //偏置
                return sigmoid(tmp);
            }

            //获取训练数据 输入输出
            void getTrainData()
            {
                int i, j;
                //读取文本，获取样本
                //getTxtData("trainDataIn.txt", *train_In, N, In);   //输入值
                //getTxtData("trainDataOut.txt", *train_Out, N, Out);  //输出值
                


                for (i = 0; i < N; i++)
                {
                    for (j = 0; j < In; j++)
                    {
                        //printf("%lf ", train_In[i][j]);     //打印样本
                        train_In[i,j] /= In_divisor;       //保持所有输入数据在-1~1内       
                    }
                    for (j = 0; j < Out; j++)
                    {
                        //printf("%lf ", train_Out[i][j]);     //打印样本
                        train_Out[i,j] /= Out_divisor;      //保持所有输出数据在-1~1内         
                    }
                    //printf("\r\n");
                }
            }

            //训练times次
            void train(int times)
            {
                int i, j, k, n;
                for (n = 0; n < times; n++)
                {
                    Err_Sum = 0;
                    double[] delta_err = new double [N];
                    for (i=0;i<N;i++)
                    { delta_err[i]=0; }
                    
                    //正向传输
                    for (i = 0; i < N; i++)
                    {
                        //计算隐藏层输出 in -> hide
                        for (j = 0; j < Hide; j++)
                        {
                            hide_Out[i,j] = getOneNodeValue(train_In[i], In, *weight_IH + j, Hide, bias_H[j]);
                        }
                        //计算输出层输出 hide -> out
                        for (j = 0; j < Out; j++)
                        {
                            BP_Out[i,j] = getOneNodeValue(hide_Out[i], Hide, *weight_HO + j, Out, bias_O[j]);
                            if ((times - n) == 1)
                                richTextBox1.AppendText( BP_Out[i,j] * Out_divisor+ "\r\n"); //最后一次训练打印每组输出
                        }
                        //计算样本误差 
                        for (j = 0; j < Out; j++)
                        {
                            delta_err[i] = train_Out[i,j] - BP_Out[i,j]; //误差
                            error[i] += delta_err[i] * delta_err[i];  //误差平方和
                        }
                        error[i] /= 2;
                        //计算隐藏层输出层间权值调整系数  有公式
                        for (j = 0; j < Out; j++)
                        {
                            delta_HO[i,j] = delta_err[i] * BP_Out[i,j] * (1 - BP_Out[i,j]);
                        }
                        //计算输入层到隐藏层的权值调整系数  有公式
                        for (j = 0; j < Hide; j++)
                        {
                            delta_IH[i,j] = 0; //清0
                            for (k = 0; k < Out; k++)
                            {
                                delta_IH[i,j] += delta_HO[i,k] * weight_HO[j,k] * hide_Out[i,j] * (1 - hide_Out[i,j]);
                            }
                        }
                        Err_Sum += error[i]; //计算
                    }
                    //反向传输
                    //调整weight_IH
                    double temp = 0.0;
                    for (i = 0; i < In; i++)
                    {
                        for (j = 0; j < Hide; j++)
                        {
                            temp = 0;
                            for (k = 0; k < N; k++)
                            {
                                temp += delta_IH[k,j] * train_In[k,i];
                            }
                            weight_IH[i,j] += rate * temp;

                        }
                    }
                    //调整bias_H
                    for (i = 0; i < Hide; i++)
                    {
                        temp = 0;
                        for (j = 0; j < N; j++)
                        {
                            temp -= delta_IH[j,i];
                        }
                        bias_H[i] += rate * temp;
                    }
                    //调整weight_HO
                    for (i = 0; i < Hide; i++)
                    {
                        for (j = 0; j < Out; j++)
                        {
                            temp = 0;
                            for (k = 0; k < N; k++)
                            {
                                temp += delta_HO[k,j] * hide_Out[k,i];
                            }
                            weight_HO[i,j] += rate * temp;
                        }
                    }
                    //调整bias_O
                    for (i = 0; i < Out; i++)
                    {
                        temp = 0;
                        for (j = 0; j < N; j++)
                        {
                            temp -= delta_HO[j,i];
                        }
                        bias_O[i] += rate * temp;
                    }
                    if (Err_Sum < Err_max) break; //误差到达允许值 跳出循环 停止训练
                    if ((n % (times / 10)) == 0)
                    { //每Times/10次打印一次信息
                        richTextBox1.AppendText("次数:" + n + times / 10+" \r误差:"+ Err_Sum+"\r\n");
                        //printf("----------------------------------------------------------\r\n");
                    }
                }
            }

            //输入(处理后的 / In_divisor)测试数据首地址 输出数据首地址
            void test(double* data, double* output)
            {
                int i;
                double[] hide = new double[Hide];
                for (i = 0; i < N; i++)
                { hide[i] = 0; }

                for (i = 0; i < Hide; i++)
                {
                    hide[i] = getOneNodeValue(data, In, *weight_IH + i, Hide, bias_H[i]);
                }
                for (i = 0; i < Out; i++)
                {
                    output[i] = getOneNodeValue(hide, Hide, *weight_HO + i, Out, bias_O[i]) * Out_divisor;
                }
            }

            //从文本获取数据 文本名 首地址 行数 列数
            void getTxtData(char* FileName, double* Data, int Row, int Column)
            {
                int i, j;
                FILE* fp;
                fp = fopen(FileName, "r");
                if (fp == NULL)
                {
                    printf("读取失败\r\n");
                    return;
                }
                for (i = 0; i < Row; i++)
                {
                    SetPositionByLine(fp, i); //文本光标转至第i行
                    for (j = 0; j < Column; j++)
                    {
                        fscanf(fp, "%lf ", Data + i * Column + j); //读数据
                    }
                }
                fclose(fp);
            }

            //保存数据至文本 文本名 首地址 行数 列数
            void saveTxtData(char* FileName, double* Data, int Row, int Column)
            {
                int i, j;
                FILE* fp;
                fp = fopen(FileName, "w+");
                if (fp == NULL)
                {
                    printf("创建失败\r\n");
                    return;
                }
                for (i = 0; i < Row; i++)
                {
                    for (j = 0; j < Column; j++)
                    {
                        fprintf(fp, "%lf ", *(Data + i * Column + j)); //写数据
                    }
                    fprintf(fp, "\n"); //换行     
                }
                fclose(fp);
            }



            int main()
            {
                int i, j, mod = 0;
                double [,] testData= new double[Ntest,In];
                double [,] OutData= new double [Ntest,Out];
                parm_init(); //参数初始化
                //printf("是否读取参数 1: 读取 0: 不读取\r\n");
                //scanf("%d", &mod);
                mod = 0;
                if (mod == 1)
                {
                    //读取参数
                    //getTxtData("weight_IH.txt", *weight_IH, In, Hide);
                   // getTxtData("weight_HO.txt", *weight_HO, Hide, Out);
                    //getTxtData("bias_H.txt", bias_H, Hide, 1);
                    //getTxtData("bias_O.txt", bias_O, Out, 1);
                }

                //printf("是否训练 1: 训练 0: 不训练\r\n");
                //scanf("%d", &mod);
                mod = 1;
                if (mod == 1)
                {
                    getTrainData();
                    //printf("训练次数：\r\n");
                    //scanf("%d", &Times);
                    train(Times);
                    richTextBox1.AppendText("-------------------------训练完成-------------------------\r\n");
                    richTextBox1.AppendText("\r\n训练完毕 误差:"+ Err_Sum +"\r\n" );
                }

                getTxtData("testData.txt", *testData, Ntest, In); //获取测试数据
                for (i = 0; i < In; i++)
                {
                    for (j = 0; j < Ntest; j++)
                    {
                        testData[j,i] /= In_divisor;  //处理输入
                    }
                }

                richTextBox1.AppendText("\r\n--------------------------测试集--------------------------\r\n");
                for (i = 0; i < Ntest; i++)
                {
                    test(testData[i], OutData[i]);
                    for (j = 0; j < Out; j++)
                    {
                        richTextBox1.AppendText(OutData[i,j]);
                    }
                    //printf("\r\n");
                }
                //存储参数



                //saveTxtData("weight_IH.txt", *weight_IH, In, Hide);
                //saveTxtData("weight_HO.txt", *weight_HO, Hide, Out);
                //saveTxtData("bias_H.txt", bias_H, Hide, 1);
                //saveTxtData("bias_O.txt", bias_O, Out, 1);
                string path=@"C:\Users\fair yang\Desktop\bias_O.txt";
                System.IO.File.WriteAllLines(path, new string[] { "123456","098765" });
                return 0;
            }
          */
            



        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
