using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeBoard_Task1;
using SafeBoard_Task1.Contacts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        public void LockedFileScanTest()
        {
            var rules = new ScannerRule[]
            {
                TestsEnviroment.JsRule,
                TestsEnviroment.RmRule,
                TestsEnviroment.Rundll32Rule
            };

            var description = new ReportDescription();

            description.Reports = new Dictionary<DetectionReportType, int>()
            {
                { DetectionReportType.Clean, 2 },
                { DetectionReportType.Skipped, 0 },
                { DetectionReportType.Malvare, 2 },
                { DetectionReportType.NoAccess, 0 },
                { DetectionReportType.Error, 2 },
                { DetectionReportType.FileNotExists, 0 },
            };
            description.Rules = new Dictionary<string, int>()
            {
                { TestsEnviroment.JsRule.RuleName, 0 },
                { TestsEnviroment.RmRule.RuleName, 1 },
                { TestsEnviroment.Rundll32Rule.RuleName, 1 }
            };

            using (var jsFile = File.OpenRead(TestsEnviroment.JsMalvareFilePath))
            using (var cleanFile = File.OpenRead(TestsEnviroment.CleanFilePath))
            {
                jsFile.Lock(0, 1);
                cleanFile.Lock(0, 1);

                BaseScanTest(TestsEnviroment.AffectedDirectory, rules, description);
            }
        }

        [TestMethod]
        public void DefaultScanTest()
        {
            var rules = new ScannerRule[]
            {
                TestsEnviroment.JsRule,
                TestsEnviroment.RmRule,
                TestsEnviroment.Rundll32Rule
            };

            var description = new ReportDescription();

            description.Reports = new Dictionary<DetectionReportType, int>()
            {
                { DetectionReportType.Clean, 3 },
                { DetectionReportType.Skipped, 0 },
                { DetectionReportType.Malvare, 3 },
                { DetectionReportType.NoAccess, 0 },
                { DetectionReportType.Error, 0 },
                { DetectionReportType.FileNotExists, 0 },
            };
            description.Rules = new Dictionary<string, int>()
            {
                { TestsEnviroment.JsRule.RuleName, 1 },
                { TestsEnviroment.RmRule.RuleName, 1 },
                { TestsEnviroment.Rundll32Rule.RuleName, 1 }
            };

            BaseScanTest(TestsEnviroment.AffectedDirectory, rules, description);
        } 

        private void BaseScanTest(string directory, ScannerRule[] rules, ReportDescription expected)
        {
            var scanner = new Scanner(rules);
            scanner.Scan(directory);

            foreach (var description in expected.Reports)
            {
                Assert.AreEqual(description.Value, scanner.ReportInfo.GetReportsByType(description.Key).Count(),
                    $"Unexpected {description.Key} reports count");
            }

            var malwareReports = scanner.ReportInfo.GetReportsByType(DetectionReportType.Malvare);

            foreach (var rule in expected.Rules)
            {
                Assert.AreEqual(rule.Value, malwareReports.Count(report => report.Rule.RuleName == rule.Key),
                    $"Unexpected {rule.Key} rull reports count");
            }
        }
    }
}
