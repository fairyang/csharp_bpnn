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
using WinFormsApp1;
//using Class1;

namespace WinFormsApp1
{
     public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
                
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("test");
            double a1 = 468.9;
            double a2 = 487.1;
            double a3 = 472.7;
            double end;
            end=Class1.Hello( a1,a2,a3);
            richTextBox1.AppendText("\n"+end+"\n");

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
