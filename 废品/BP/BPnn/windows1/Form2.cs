using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace windows1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            BarMap();
        }

        private void BitMap()//饼状图
        {

            int[] saleNum = { 300, 500, 400 };
            int sum = 0, threeNum = 0, fourNum = 0, fiveNum = 0;
            for (int i = 0; i < saleNum.Length; i++)
            {
                sum += saleNum[i];
                if (i == 0)
                    threeNum = saleNum[0];
                else if (i == 1)
                    fourNum = saleNum[1];
                else
                    fiveNum = saleNum[2];
            }

            int height = pictureBox1.Height, width = pictureBox1.Width;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            Pen pen1 = new Pen(Color.Red);
            Brush brush1 = new SolidBrush(Color.PowderBlue);
            Brush brush2 = new SolidBrush(Color.Blue);
            Brush brush3 = new SolidBrush(Color.Wheat);
            Brush brush4 = new SolidBrush(Color.Orange);

            Font font1 = new Font("Couriter New", 16, FontStyle.Bold);
            Font font2 = new Font("Couriter New", 10);
            g.FillRectangle(brush1, 0, 0, width, height);
            g.DrawString("每月销售占比饼状图", font1, brush2, new Point(70, 20));
            int piex = 100, piey = 60, piew = 200, pieh = 200;
            float angle1 = Convert.ToSingle((360 / Convert.ToSingle(sum)) * Convert.ToSingle(threeNum));
            float angle2 = Convert.ToSingle((360 / Convert.ToSingle(sum)) * Convert.ToSingle(fourNum));
            float angle3 = Convert.ToSingle((360 / Convert.ToSingle(sum)) * Convert.ToSingle(fiveNum));
            g.FillPie(brush2, piex, piey, piew, pieh, 0, angle1);
            g.FillPie(brush3, piex, piey, piew, pieh, angle1, angle2);
            g.FillPie(brush4, piex, piey, piew, pieh, angle1 + angle2, angle3);
            g.DrawRectangle(pen1, 50, 300, 310, 130);


            g.FillRectangle(brush2, 90, 320, 20, 10);
            g.DrawString(string.Format("3月份销量占比：{0:P2}", Convert.ToSingle(threeNum) / Convert.ToSingle(sum)), font2, brush2, 120, 320);
            g.FillRectangle(brush3, 90, 360, 20, 10);
            g.DrawString(string.Format("4月份销量占比：{0:P2}", Convert.ToSingle(fourNum) / Convert.ToSingle(sum)), font2, brush2, 120, 360);
            g.FillRectangle(brush4, 90, 400, 20, 10);
            g.DrawString(string.Format("5月份销量占比：{0:P2}", Convert.ToSingle(fiveNum) / Convert.ToSingle(sum)), font2, brush2, 120, 400);

            this.groupBox1.Text = "饼状图";
            this.pictureBox1.Width = bitmap.Width;
            this.pictureBox1.Height = bitmap.Height;
            this.pictureBox1.BackgroundImage = bitmap;

        }
        private void BarMap()//柱状图
        {
            int[] saleNum = { 300, 500, 400 };
            int sum = saleNum[0] + saleNum[1] + saleNum[2];
            float[] Y_Num ={ Convert.ToSingle(saleNum[0]) / Convert.ToSingle(sum),Convert.ToSingle(saleNum[1]) / Convert.ToSingle(sum),
                    Convert.ToSingle(saleNum[2]) / Convert.ToSingle(sum) };
            int height = pictureBox1.Height, width = pictureBox1.Width;
            Bitmap image = new Bitmap(width, height);
            //创建Graphics类对象
            Graphics g = Graphics.FromImage(image);
            try
            {
                //清空图片背景色
                g.Clear(Color.White);
                Font font = new Font("Arial", 10, FontStyle.Regular);
                Font font1 = new Font("宋体", 20, FontStyle.Bold);
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.BlueViolet, 1.2f, true);

                Font font2 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                SolidBrush mybrush = new SolidBrush(Color.Red);
                SolidBrush mybrush2 = new SolidBrush(Color.Green);
                Pen mypen = new Pen(brush, 1);
                //绘制线条
                //绘制横向线条
                int x = 100;
                Pen mypen1 = new Pen(Color.Blue, 2);
                x = 60;
                g.DrawLine(mypen1, x, 0, x, 300);

                //绘制纵向线条
                int y = 0;
                for (int i = 0; i < 11; i++)
                {
                    g.DrawLine(mypen, 45, y, 60, y);
                    y = y + 30;
                }
                g.DrawLine(mypen1, 60, y - 30, 620, y - 30);

                //x轴
                String[] n = { "3月份", "4月份", "5月份" };
                x = 100;
                for (int i = 0; i < 3; i++)
                {
                    g.DrawString(n[i].ToString(), font, Brushes.Blue, x, 300); //设置文字内容及输出位置

                    Console.WriteLine(300 - Y_Num[i] * 100 * 3);
                    g.FillRectangle(mybrush, x, 300 - Y_Num[i] * 100 * 3, 20, Y_Num[i] * 100 * 3);
                    g.DrawString(Y_Num[i].ToString(), font2, Brushes.Green, x, 300 - Y_Num[i] * 100 * 3 - 15);

                    x = x + 100;
                }

                //y轴
                String[] m = { "0", "0.10", "0.20", "0.30", "0.40", "0.50", "0.60", "0.70", " 0.80", " 0.90", " 1.00" };
                y = 0;
                for (int i = 10; i >= 0; i--)
                {
                    g.DrawString(m[i].ToString(), font, Brushes.Blue, 20, y); //设置文字内容及输出位置
                    y = y + 30;
                }
                //绘制标识
                Font font3 = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
                g.DrawRectangle(new Pen(Brushes.Blue), 170, 390, 250, 50); //绘制范围框
                g.FillRectangle(Brushes.Red, 200, 410, 20, 10); //绘制小矩形
                g.DrawString("月销量占比", font3, Brushes.Red, 292, 408);
                this.button1.Text = "查看饼状图";
                this.groupBox1.Text = "柱状图";
                this.pictureBox1.Width = image.Width;
                this.pictureBox1.Height = image.Height;
                this.pictureBox1.BackgroundImage = image;
            }
            catch { }

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            BitMap();
            this.button1.Visible = false;//隐藏button
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
