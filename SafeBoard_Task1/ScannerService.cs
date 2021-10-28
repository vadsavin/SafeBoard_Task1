using SafeBoard_Task1.Contacts;
using System;
using System.Linq;
using System.Text;
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
        /// </summary>
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
        public string Run()
        {
            var scanner = new Scanner(_defaultRules);
            scanner.Scan(DirectoryToScan);

            return GenereteReport(scanner.ReportInfo);
        }

        /// <summary>
        /// Запуск сканирования.
        /// </summary>
        public Task RunScannerAsync(out Scanner scanner)
        {
            scanner = new Scanner(_defaultRules);
            return scanner.ScanAsync(DirectoryToScan);
        }

        /// <summary>
        /// Генерирует строку отчёта по результатми сканирования
        /// </summary>
        public string GenereteReport(ReportInfo info)
        {
            var resultBuilder = new StringBuilder("====== Scan result ======\n");

            resultBuilder.AppendLine($"Processed files: {info.GetAmountOfReports()}");

            var malvares = info.GetReportsByType(DetectionReportType.Malvare);
            foreach (var rule in _defaultRules)
            {
                var malvaresByRule = malvares.Where(report => report.Rule == rule);
                resultBuilder.AppendLine($"{rule.RuleName} detects: {malvaresByRule.Count()}");
            }

            var noAccessReports = info.GetReportsByType(DetectionReportType.NoAccess);
            var noFileReports = info.GetReportsByType(DetectionReportType.FileNotExists);
            var errorReports = info.GetReportsByType(DetectionReportType.Error);

            int allErrorSum = noAccessReports.Count() + noFileReports.Count() + errorReports.Count();
            resultBuilder.AppendLine($"Errors: {allErrorSum}");

            resultBuilder.AppendLine($"Exection time: {info.ScanningTime}");

            resultBuilder.AppendLine("=========================");

            return resultBuilder.ToString();
        }
    }
}
