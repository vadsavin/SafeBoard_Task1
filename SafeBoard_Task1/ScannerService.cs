using SafeBoard_Task1.Contacts;
using System;
using System.Threading.Tasks;

namespace SafeBoard_Task1
{
    public class ScannerService
    {
        //Дефолтные значения правил сканирования, созданные по заданию
        private static readonly ScannerRule[] _defaultRules = new ScannerRule[] 
        { 
            new ScannerRule("JS", @".*\.js\Z", "<script>evil_script()</script>"),
            new ScannerRule("rm -rf", @"rm -rf %userprofile%\Documents"),
            new ScannerRule("Rundll32", "Rundll32 sus.dll SusEntry")
        };

        /// <summary>
        /// Директория для сканирования (поддиректории также подвергаются сканированию)
        /// </summary>s
        public string DirectoryToScan { get; set; }

        public ScannerService(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException();
            }
            else
            {
                DirectoryToScan = args[0];
            }
        }

        /// <summary>
        /// Запуск сканирования.
        /// </summary>
        public string RunScanner()
        {
            var scanner = new Scanner(_defaultRules);
            scanner.Scan(DirectoryToScan);

            return ReportBuilder.GenereteReport(scanner.ReportInfo);
        }

        /// <summary>
        /// Запуск ассинхронного сканирования.
        /// </summary>
        public Task RunScannerAsync(out Scanner scanner)
        {
            scanner = new Scanner(_defaultRules);
            return scanner.ScanAsync(DirectoryToScan);
        }

        
    }
}
