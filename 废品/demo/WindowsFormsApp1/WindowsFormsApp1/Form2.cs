using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//连接数据库
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //连接数据库，这里用的是mysql
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
                 richTextBox1.AppendText("连接数据库失败\n"+ex.Message);
             }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
