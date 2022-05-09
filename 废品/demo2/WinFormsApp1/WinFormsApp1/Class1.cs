using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinFormsApp1
{
    class Class1
    {
        //调用test.dll
        [DllImport("test1.dll")]
        //[DllImport("Test1.dll", EntryPoint = "Hello", CharSet = CharSet.Ansi)]
        public static extern double Hello(double a1,double a2,double a3);
        //public static extern int Hello();
        //public static extern int FunT1(int a, int b);
    }
}
