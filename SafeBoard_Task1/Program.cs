using System;
using System.Linq;
using System.Threading;

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

        //static void Main(string[] args)
        //{
        //    var service = new ScannerService(new[] { "F:\\repos\\SafeBoard_Task1" });

        //    var scannerTask = service.RunScannerAsync(out var scanner);

        //    Console.WriteLine(service.GenereteReport(scanner.ReportInfo));

        //    Console.ReadKey();
        //}
    }
}
