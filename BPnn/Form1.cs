using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.Windows.Forms.DataVisualization.Charting;






namespace BPnn
{
    public partial class Form1 : Form
    {
        public Form1()
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
                richTextBox1.AppendText(Time + "  " + "\n");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                richTextBox1.AppendText("连接数据库失败\n" + ex.Message);
                //输出时间
                string Time = Convert.ToString(DateTime.Now);
                richTextBox1.AppendText(Time + "  " + "\n");
            }
            return conn;
        }
        public float getdata(string table, int id, MySqlConnection conn)
        {
            //从数据库中取数据
            float ans;
            string sql1 = "select temperature from " + table + " where id = " + id;
            //string sql1 = "select " + item + " from " + table + " where id =" + id;
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            ans = float.Parse(cmd1.ExecuteScalar().ToString());
            richTextBox1.AppendText($"读取数据: {ans}\n");

            return ans;
        }
        public void insertdata(double d, int id, string table, MySqlConnection conn)
        {            
            string sql = "update "+table+" set temperature = " + d + " where id=" + id;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                richTextBox1.AppendText("成功写入数据库\n");
            }


        }

        public void bar1(double [] x, double [] y)
        {

            #region 柱状图1

            //标题            
            chart1.Titles.Add("柱状图数据分析");
            chart1.Titles[0].ForeColor = Color.Black;
            chart1.Titles[0].Font = new Font("宋体", 10f, FontStyle.Regular);
            chart1.Titles[0].Alignment = ContentAlignment.TopCenter;

            //右上角标题
            chart1.Titles.Add("101-150");                     
            chart1.Titles[1].ForeColor = Color.Black;
            chart1.Titles[1].Font = new Font("宋体", 10f, FontStyle.Regular);
            //设置标题位于右上角
            //chart1.Titles[1].Alignment = ContentAlignment.TopRight;
            //chart1.Titles[2].Alignment = ContentAlignment.TopRight;
            //chart1.Titles[3].Alignment = ContentAlignment.TopRight;
            //chart1.Titles[4].Alignment = ContentAlignment.TopRight;
            
            //控件背景
            chart1.BackColor = Color.White;
            //图表区背景
            chart1.ChartAreas[0].BackColor = Color.Transparent;
            chart1.ChartAreas[0].BorderColor = Color.Transparent;
            //X轴标签间距
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;//是否偏移
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 0;   //下方标签显示的角度 -45为倾斜45度
            
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("宋体",5f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;


            //X坐标轴颜色
            chart1.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("宋体", 10f, FontStyle.Regular);
            //X坐标轴标题
            chart1.ChartAreas[0].AxisX.Title = "编号";
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart1.ChartAreas[0].AxisX.ToolTip = "编号";
            //X轴网络线条
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            //Y坐标轴颜色
            chart1.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("宋体", 10f, FontStyle.Regular);
            //Y坐标轴标题
            chart1.ChartAreas[0].AxisY.Title = "温度";
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;
            chart1.ChartAreas[0].AxisY.ToolTip = "温度";
            //Y轴网格线条
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            chart1.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            chart1.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            string now_time = System.DateTime.Now.ToString("d");
            Legend legend = new Legend(now_time);
            legend.Title = "legendTitle";
                     
            chart1.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            //chart1.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart1.Series[0].LabelForeColor = Color.Black;
            //chart1.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart1.Series[0].ChartType = SeriesChartType.Column;    //图类型(折线)

            chart1.Series[0].Color = Color.Lime;
            chart1.Series[0].LegendText = legend.Name;
            chart1.Series[0].IsValueShownAsLabel = false;//柱状图上面添加数字
            chart1.Series[0].LabelForeColor = Color.Black;
            chart1.Series[0].CustomProperties = "DrawingStyle = Cylinder";            
            chart1.Legends.Add(legend);
            chart1.Legends[0].Position.Auto = false ;          
            

            //绑定数据
            chart1.Series[0].Points.DataBindXY(x, y);
            chart1.Series[0].Points[0].Color = Color.Black;
            chart1.Series[0].Palette = ChartColorPalette.Bright;

            #endregion
        }

        public void bar2(double[] x, double[] y)
        {
            #region 柱状图2

            
            //标题
            chart2.Titles.Add("柱状图数据分析");
            chart2.Titles[0].ForeColor = Color.Black;
            chart2.Titles[0].Font = new Font("宋体", 10f, FontStyle.Regular);
            chart2.Titles[0].Alignment = ContentAlignment.TopCenter;

            //右上角标题
            chart2.Titles.Add("201-250");
            
            chart2.Titles[1].ForeColor = Color.Black;
            chart2.Titles[1].Font = new Font("宋体", 10f, FontStyle.Regular);
            //设置标题位于右上角
            
            //控件背景
            chart2.BackColor = Color.White;
            //图表区背景
            chart2.ChartAreas[0].BackColor = Color.Transparent;
            chart2.ChartAreas[0].BorderColor = Color.Transparent;
            //X轴标签间距
            chart2.ChartAreas[0].AxisX.Interval = 1;
            chart2.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;
            chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 0;   //下方标签显示的角度
            chart2.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart2.ChartAreas[0].AxisX.TitleForeColor = Color.Black;

            //X坐标轴颜色
            chart2.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            chart2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            chart2.ChartAreas[0].AxisX.LabelStyle.Font = new Font("宋体", 10f, FontStyle.Regular);
            //X坐标轴标题
            chart2.ChartAreas[0].AxisX.Title = "编号";
            chart2.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart2.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            chart2.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart2.ChartAreas[0].AxisX.ToolTip = "编号";
            //X轴网络线条
            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            //Y坐标轴颜色
            chart2.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            chart2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            chart2.ChartAreas[0].AxisY.LabelStyle.Font = new Font("宋体", 5f, FontStyle.Regular);
            //Y坐标轴标题
            chart2.ChartAreas[0].AxisY.Title = "温度";
            chart2.ChartAreas[0].AxisY.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart2.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            chart2.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;
            chart2.ChartAreas[0].AxisY.ToolTip = "温度";
            //Y轴网格线条
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            chart2.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            chart2.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            string now_time = System.DateTime.Now.ToString("d");
            Legend legend = new Legend(now_time);
            legend.Title = "legendTitle";

            chart2.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            //chart2.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart2.Series[0].LabelForeColor = Color.Black;
            //chart2.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart2.Series[0].ChartType = SeriesChartType.Column;    //图类型(折线)


            chart2.Series[0].Color = Color.Lime;
            chart2.Series[0].LegendText = legend.Name;
            chart2.Series[0].IsValueShownAsLabel = false;
            chart2.Series[0].LabelForeColor = Color.Black;
            chart2.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            chart2.Legends.Add(legend);
            chart2.Legends[0].Position.Auto = false;


            //绑定数据
            chart2.Series[0].Points.DataBindXY(x, y);
            chart2.Series[0].Points[0].Color = Color.Black;
            chart2.Series[0].Palette = ChartColorPalette.Bright;

            #endregion
        }

        public void bar3(double[] x, double[] y)
        {

            #region 柱状图3

            //标题
            chart3.Titles.Add("柱状图数据分析");
            chart3.Titles[0].ForeColor = Color.Black;
            chart3.Titles[0].Font = new Font("宋体", 10f, FontStyle.Regular);
            chart3.Titles[0].Alignment = ContentAlignment.TopCenter;

            //右上角标题
            chart3.Titles.Add("301-350");
            //chart3.Titles.Add("合计：25414 宗");
            //chart3.Titles.Add("合计：25414 宗");
            //chart3.Titles.Add("合计：25414 宗");
            chart3.Titles[1].ForeColor = Color.Black;
            chart3.Titles[1].Font = new Font("宋体", 10f, FontStyle.Regular);
            //设置标题位于右上角
            //chart3.Titles[1].Alignment = ContentAlignment.TopRight;
            //chart3.Titles[2].Alignment = ContentAlignment.TopRight;
            //chart3.Titles[3].Alignment = ContentAlignment.TopRight;
            //chart3.Titles[4].Alignment = ContentAlignment.TopRight;

            //控件背景
            chart3.BackColor = Color.White;
            //图表区背景
            chart3.ChartAreas[0].BackColor = Color.Transparent;
            chart3.ChartAreas[0].BorderColor = Color.Transparent;
            //X轴标签间距
            chart3.ChartAreas[0].AxisX.Interval = 1;
            chart3.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;
            chart3.ChartAreas[0].AxisX.LabelStyle.Angle = 0;   //下方标签显示的角度
            chart3.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart3.ChartAreas[0].AxisX.TitleForeColor = Color.Black;

            //X坐标轴颜色
            chart3.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            chart3.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            chart3.ChartAreas[0].AxisX.LabelStyle.Font = new Font("宋体", 10f, FontStyle.Regular);
            //X坐标轴标题
            chart3.ChartAreas[0].AxisX.Title = "编号";
            chart3.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart3.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            chart3.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart3.ChartAreas[0].AxisX.ToolTip = "编号";
            //X轴网络线条
            chart3.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart3.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            //Y坐标轴颜色
            chart3.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            chart3.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            chart3.ChartAreas[0].AxisY.LabelStyle.Font = new Font("宋体", 10f, FontStyle.Regular);
            //Y坐标轴标题
            chart3.ChartAreas[0].AxisY.Title = "厚度";
            chart3.ChartAreas[0].AxisY.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart3.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            chart3.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;
            chart3.ChartAreas[0].AxisY.ToolTip = "厚度";
            //Y轴网格线条
            chart3.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart3.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            chart3.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            chart3.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            string now_time = System.DateTime.Now.ToString("d");
            Legend legend = new Legend(now_time);
            legend.Title = "legendTitle";

            chart3.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            //chart3.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart3.Series[0].LabelForeColor = Color.Black;
            //chart3.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart3.Series[0].ChartType = SeriesChartType.Column;    //图类型(折线)


            chart3.Series[0].Color = Color.Lime;
            chart3.Series[0].LegendText = legend.Name;
            chart3.Series[0].IsValueShownAsLabel = false;
            chart3.Series[0].LabelForeColor = Color.Black;
            chart3.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            chart3.Legends.Add(legend);
            chart3.Legends[0].Position.Auto = false;


            //绑定数据
            chart3.Series[0].Points.DataBindXY(x, y);
            chart3.Series[0].Points[0].Color = Color.Black;
            chart3.Series[0].Palette = ChartColorPalette.Bright;

            #endregion

        }
        public void bar4(double[] x, double[] y)
        {
           
            #region 柱状图4

            //标题
            chart4.Titles.Add("柱状图数据分析");
            chart4.Titles[0].ForeColor = Color.Black;
            chart4.Titles[0].Font = new Font("宋体", 10f, FontStyle.Regular);
            chart4.Titles[0].Alignment = ContentAlignment.TopCenter;

            //右上角标题
            chart4.Titles.Add("401-450");
            
            chart4.Titles[1].ForeColor = Color.Black;
            chart4.Titles[1].Font = new Font("宋体", 10f, FontStyle.Regular);
            //设置标题位于右上角
            

            //控件背景
            chart4.BackColor = Color.White;
            //图表区背景
            chart4.ChartAreas[0].BackColor = Color.Transparent;
            chart4.ChartAreas[0].BorderColor = Color.Transparent;
            //X轴标签间距
            chart4.ChartAreas[0].AxisX.Interval = 1;
            chart4.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;
            chart4.ChartAreas[0].AxisX.LabelStyle.Angle = 0;   //下方标签显示的角度
            chart4.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart4.ChartAreas[0].AxisX.TitleForeColor = Color.Black;

            //X坐标轴颜色
            chart4.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            chart4.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            chart4.ChartAreas[0].AxisX.LabelStyle.Font = new Font("宋体", 10f, FontStyle.Regular);
            //X坐标轴标题
            chart4.ChartAreas[0].AxisX.Title = "编号";
            chart4.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart4.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            chart4.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart4.ChartAreas[0].AxisX.ToolTip = "编号";
            //X轴网络线条
            chart4.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart4.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            //Y坐标轴颜色
            chart4.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            chart4.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            chart4.ChartAreas[0].AxisY.LabelStyle.Font = new Font("宋体", 10f, FontStyle.Regular);
            //Y坐标轴标题
            chart4.ChartAreas[0].AxisY.Title = "温度";
            chart4.ChartAreas[0].AxisY.TitleFont = new Font("宋体", 10f, FontStyle.Regular);
            chart4.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            chart4.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;
            chart4.ChartAreas[0].AxisY.ToolTip = "温度";
            //Y轴网格线条
            chart4.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart4.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");

            chart4.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            chart4.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            string now_time = System.DateTime.Now.ToString("d");
            Legend legend = new Legend(now_time);
            legend.Title = "legendTitle";

            chart4.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            //chart4.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart4.Series[0].LabelForeColor = Color.Black;
            //chart4.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart4.Series[0].ChartType = SeriesChartType.Column;    //图类型(折线)


            chart4.Series[0].Color = Color.Lime;
            chart4.Series[0].LegendText = legend.Name;
            chart4.Series[0].IsValueShownAsLabel = false;
            chart4.Series[0].LabelForeColor = Color.Black;
            chart4.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            chart4.Legends.Add(legend);
            chart4.Legends[0].Position.Auto = false;


            //绑定数据
            chart4.Series[0].Points.DataBindXY(x, y);
            chart4.Series[0].Points[0].Color = Color.Black;
            chart4.Series[0].Palette = ChartColorPalette.Bright;

            #endregion

        }

        public void show(int t, MySqlConnection conn)
        {
            //画图，t表示第几个图          

            t = t * 100;
            string table="";

            if (t == 100)
            {
                table = "data1";
            }
            if (t == 200)
            {
                table = "data2";
                
            }
            if (t == 300)
            {
                table = "data3";
            }
            if (t == 400)
            {
                table = "data4";
            }
                        
            float ans;            
            int n = 50;
            double[] x = new double[n];
            double[] y = new double[n];
            for (int i = 1; i <= n; i++)
            {
                int id = t + i;

                ans = getdata(table, id, conn);
                x[i - 1] = id;
                y[i - 1] = ans;
            }

            /*double a1, a2, a3;
            double d;
            for (int i = t+ 1; i <= t + n; i = i + 3)
            {
                a1 = getdata("temperature", table , i, conn);
                richTextBox1.AppendText($"\nPredicted: {a1}\n");

                a2 = getdata("temperature", table , i + 1, conn);
                richTextBox1.AppendText($"\nPredicted: {a2}\n");

                a3 = getdata("temperature", table , i + 2, conn);
                richTextBox1.AppendText($"\nPredicted: {a3}\n");

                d = Class1.Hello(a1, a2, a3);
                richTextBox1.AppendText($"\nPredicted Col3: {d}\n");

                //将 d 后小数点位数指定为2（四舍五入）
                d = Math.Round((double)d, 2);
                x[i - 1] = i;
                y[i - 1] = d;
            }*/
            if (t==100)
            {
                bar1(x, y);                
            }            
             if(t==200)
            {                
                bar2(x,y);
            }
            if (t==300)
            {
                bar3(x, y);
            }
            if (t==400)
            {
                bar4(x, y);
            }                  

        }

        public void predict(int t, MySqlConnection conn)
        {
            //整体预测
            t = t * 100;
            string table = "";
            string table1= "";
            if (t == 100)
            {
                table = "data1";
                table1 = "predict1";
            }
            if (t == 200)
            {
                table = "data2";
                table1 = "predict2";

            }
            if (t == 300)
            {
                table = "data3";
                table1 = "predict3";
            }
            if (t == 400)
            {
                table = "data4";
                table1 = "predict4";
            }
            
            double a1=0, a2=0, a3=0,d=0;
            //奇数
            for (int i=t+3;i<t+49;i=i+2)
            {
                a1 = getdata(table, i-2, conn);
                a2 = getdata(table, i, conn);
                a3 = getdata(table, i+2, conn);
                d = Class1.Hello(a1, a2, a3);
                richTextBox1.AppendText($"Predicted: {d}\n");
                insertdata(d,i, table1, conn);
                
            }
            //偶数
            for(int i=t+4;i<t+50;i=i+2)
            {
                a1 = getdata(table, i - 2, conn);
                a2 = getdata(table, i, conn);
                a3 = getdata(table, i + 2, conn);
                d = Class1.Hello(a1, a2, a3);
                richTextBox1.AppendText($"Predicted: {d}\n");
                insertdata(d,i,table1,conn);
                
            }         


        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = SQLconnection();
            //画图
            show(1, conn);
            show(2, conn);
            show(3, conn);
            show(4, conn);
            //整体预测
            predict(1, conn);
            predict(2, conn);
            predict(3, conn);
            predict(4, conn);
                        
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void chart4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = SQLconnection();
            //读入
            int  id = int.Parse(textBox1.Text.ToString());
            double a1 = double.Parse(textBox2.Text.ToString());
            double a2 = double.Parse(textBox3.Text.ToString());
            double a3 = double.Parse(textBox4.Text.ToString());
            double d = 0;

            string table = "";
            string table1 = "";
            if(id>100 && id<200)
            {
                table = "data1";
                table1 = "predict1";
            }
            if(id>200 && id<300)
            {
                table = "data2";
                table1 = "predict2";
            }
            if(id>300 && id<400)
            {
                table = "data3";
                table1 = "predict3";
            }
            if(id>400 && id<500)
            {
                table = "data4";
                table1 = "predict4";
            }
            d = Class1.Hello(a1, a2, a3);//预测
            richTextBox1.AppendText($"本组温度为：\n");
            richTextBox1.AppendText($"第{id-2}个传感器温度为: {a1}\n");
            richTextBox1.AppendText($"第{id}个传感器温度为: {a2}\n");
            richTextBox1.AppendText($"第{id+2}个传感器温度为: {a3}\n");
            insertdata(d, id, table, conn);//写入数据库
            richTextBox1.AppendText($"预测结果: {d}\n");      
            insertdata( d, id, table1, conn);//写入数据库
            //显示一次时间
            //string Time = Convert.ToString(DateTime.Now);
            //richTextBox1.AppendText(Time + "  " + "\n");

            //页面刷新一次
            /*this.Controls.Clear();
            InitializeComponent();
                        
            //画图
            show(1, conn);
            show(2, conn);
            show(3, conn);
            show(4, conn);
            */


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();

            MySqlConnection conn = SQLconnection();
            //画图
            show(1, conn);
            show(2, conn);
            show(3, conn);
            show(4, conn);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //跳转页面
            Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
