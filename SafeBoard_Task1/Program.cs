using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SafeBoard_Task1
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    var result = new ScannerService(args).Run();
        //    Console.WriteLine(result);

        //    Console.ReadKey();
        //}

        static void Main(string[] args)
        {
            var service = new ScannerService(new[] { "F:\\repos\\SafeBoard_Task1" });

            var scannerTask = service.RunScannerAsync(out var scanner);

            Console.Clear();

            while (scanner.ReportInfo.ScanInProgress)
            {
                Console.WriteLine($"scanning...  {scanner.ReportInfo.ScanningTime}");
                Console.WriteLine($"{scanner.ReportInfo.GetAmountOfReports()} files processed.");
                Console.WriteLine($"{Math.Round(scanner.ReportInfo.BytesRead / (1024 * 1024f), 2)} total MB read.");
                Console.WriteLine($"last file: {scanner.ReportInfo.GetAllReports().LastOrDefault()?.File}" + new string(' ', 100));
                Console.SetCursorPosition(0, 0);
                Thread.Sleep(100);
            }

            Console.WriteLine(service.GenereteReport(scanner.ReportInfo));

            Console.ReadKey();
        }
    }
}
