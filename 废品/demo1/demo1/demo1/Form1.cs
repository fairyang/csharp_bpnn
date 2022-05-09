using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Demo1ML.Model;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Runtime.InteropServices;
using Demo1ML.Model;


namespace demo1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Col0 = 457.9F,
                Col1 = 469.6F,
                Col2 = 468.9F,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Col3 with predicted Col3 from sample data...\n\n");
            Console.WriteLine($"Col0: {sampleData.Col0}");
            Console.WriteLine($"Col1: {sampleData.Col1}");
            Console.WriteLine($"Col2: {sampleData.Col2}");
            Console.WriteLine($"\n\nPredicted Col3: {predictionResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
           //Console.ReadKey();



            /*String connetStr = "server=127.0.0.1;port=3308;user=root;password=123456; database=test1;";
            MySqlConnection conn = new MySqlConnection(connetStr);
            DataSet ds = new DataSet();
            string  sqltext;

           try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                Console.WriteLine("已连接数据库");
                richTextBox1.AppendText("已连接数据库\n");
                richTextBox1.AppendText("已连接数据库\n");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                richTextBox1.AppendText("连接数据库失败\n"+ex.Message);
            }

           string sql2 = "select 102b from data where time=3";
           MySqlCommand cmd2 = new MySqlCommand(sql2, conn);            
           string end = cmd2.ExecuteScalar().ToString();
           Console.WriteLine(end);
           richTextBox1.AppendText(end);






           //条形统计图和折线统计图
           void ColumnAndLine()
           {
               //设置统计图标题
               chart1.Titles.Add("条形统计图和折现统计的使用");
               //设置XY轴上面的标签
               chart1.ChartAreas[0].AxisX.Title = "月份（月）";
               chart1.ChartAreas[0].AxisY.Title = "数量（个）";
               //添加统计对象
               chart1.Series.Add("Column1");
               chart1.Series.Add("Line1");
               //设置统计对象的统计图类型
               chart1.Series["Column1"].ChartType = SeriesChartType.Column;
               chart1.Series["Line1"].ChartType = SeriesChartType.Line;
               //设置统计对象颜色
               chart1.Series["Column1"].Color = Color.Blue;
               chart1.Series["Line1"].Color = Color.Red;
               //设置统计对象粗细,单位为pixel
               chart1.Series["Column1"].BorderWidth = 5;
               chart1.Series["Line1"].BorderWidth = 3;
               //设置XY轴上的值类型
               chart1.Series["Column1"].XValueType = ChartValueType.String;
               chart1.Series["Column1"].YValueType = ChartValueType.Int64;
               chart1.Series["Line1"].XValueType = ChartValueType.String;
               chart1.Series["Line1"].YValueType = ChartValueType.Int64;

               //准备数据
               String[] AllX = { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
               int[] ColumnY = { 20, 40, 10, 50, 60, 80, 70, 90, 100, 120, 120, 110 };
               int[] LineY = { 90, 70, 80, 60, 50, 10, 40, 20, 60, 50, 10, 70 };
               //绑定数据
               chart1.Series["Column1"].Points.DataBindXY(AllX, ColumnY);
               chart1.Series["Line1"].Points.DataBindXY(AllX, LineY);
               //鼠标移动到对应点显示数值
               chart1.Series["Column1"].ToolTip = "#VALX:#VAL（个）";
               chart1.Series["Line1"].ToolTip = "#VALX:#VAL（个）";
           }
           ColumnAndLine();*/


            // [DllImport("Test1.dll", EntryPoint = "sum")];
            // int Sum(int a, int b);







        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void chart4_Click(object sender, EventArgs e)
        {

        }
    }
}
