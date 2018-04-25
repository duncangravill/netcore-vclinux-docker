using System;
using System.Runtime.InteropServices;

namespace SIMPLENETCOREAPP
{
    

    class Program
    {

        [DllImport("libSIMPLEVCLINUX.so.1.0")]
        extern static int Multiply(int x, int y);

        static void Main(string[] args)
        {
            var result = Multiply(2, 2);
            Console.WriteLine(result);
        }
    }
}
