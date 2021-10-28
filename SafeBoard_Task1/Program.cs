using System;

namespace SafeBoard_Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = new ScannerService(args).Run();
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
