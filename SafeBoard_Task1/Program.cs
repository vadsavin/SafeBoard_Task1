using System;
using System.Linq;
using System.Threading;

namespace SafeBoard_Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = new ScannerService(args).RunScanner();
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
