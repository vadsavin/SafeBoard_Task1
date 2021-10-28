using SafeBoard_Task1.Contacts;
using System.Linq;
using System.Text;

namespace SafeBoard_Task1
{
    public sealed class ReportBuilder
    {
        /// <summary>
        /// Генерирует строку отчёта по результатми сканирования
        /// </summary>
        public static string GenereteReport(ReportInfo info)
        {
            var resultBuilder = new StringBuilder("====== Scan result ======\n");

            resultBuilder.AppendLine($"Processed files: {info.GetAmountOfReports()}");

            var malvares = info.GetReportsByType(DetectionReportType.Malvare);
            foreach (var rule in info.Rules)
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
