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
using BPnnML.Model;

namespace windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*public float Predict(float a1, float a2, float a3)
        {
            //预测函数
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Col0 = a1,
                Col1 = a2,
                Col2 = a3,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            /*richTextBox1.AppendText("Using model to make single prediction -- Comparing actual Col3 with predicted Col3 from sample data...\n\n");
            richTextBox1.AppendText($"Col0: {sampleData.Col0}");
            richTextBox1.AppendText($"Col1: {sampleData.Col1}");
            richTextBox1.AppendText($"Col2: {sampleData.Col2}");
            richTextBox1.AppendText($"\n\nPredicted Col3: {predictionResult.Score}\n\n");
            richTextBox1.AppendText("=============== End of process, hit any key to finish ===============\n");
            
            return predictionResult.Score;
        }*/


        public MySqlConnection SQLconnection()
        {
            //连接数据库函数，这里用的是mysql
            String connetStr = "server=127.0.0.1;port=3308;user=root;password=123456; database=test1;";
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
        public float getdata(string item, string table, string id, MySqlConnection conn)
        {
            //从数据库中取数据
            float ans;
            string sql1 = "select " + item + " from " + table + " where time =" + id;
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            ans = float.Parse(cmd1.ExecuteScalar().ToString());
            //richTextBox1.AppendText($"\nPredicted: {ans}\n");

            return ans;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = SQLconnection();

            float a1, a2, a3;
            float d;
            a1 = getdata("101a", "data", "3", conn);
            richTextBox1.AppendText($"\nPredicted: {a1}\n");

            a2 = getdata("102b", "data", "3", conn);
            richTextBox1.AppendText($"\nPredicted: {a2}\n");

            a3 = getdata("103c", "data", "3", conn);
            richTextBox1.AppendText($"\nPredicted: {a3}\n");

            d = Predict(a1, a2, a3);
            richTextBox1.AppendText($"\nPredicted Col3: {d}\n");

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = SQLconnection();
            string sql2 = "select 102b from data where time=3";
            MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
            string end = cmd2.ExecuteScalar().ToString();
            Console.WriteLine(end);
            richTextBox1.AppendText(end);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
