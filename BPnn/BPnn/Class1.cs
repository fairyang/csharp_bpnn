using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BPnn
{
    class Class1
    {
        //调用test.dll
        [DllImport("test1.dll")]
        //[DllImport("Test1.dll", EntryPoint = "Hello", CharSet = CharSet.Ansi)]
        public static extern double Hello(double a1, double a2, double a3);
    }
}
