using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeBoard_Task1;
using SafeBoard_Task1.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class DetectorTests
    {
        [TestMethod]
        public void CleanTest()
        {
            BaseDetectorTest(
               new ScannerRule("testClean", "test"),
               TestsEnviroment.CleanFilePath,
               DetectionReportType.Clean);
        }

        [TestMethod]
        public void JsMalvareTest()
        {
            BaseDetectorTest(
               new ScannerRule("testJsMalvare", @".*\.js", @"<script>evil_script()</script>"),
               TestsEnviroment.JsMalvareFilePath,
               DetectionReportType.Malvare);
        }

        [TestMethod]
        public void JsMalvareCleanTest()
        {
            BaseDetectorTest(
               new ScannerRule("testJsCleanMalvare", @".*\.js", "<script>evil_script()</script>"),
               TestsEnviroment.JsMalvareCleanFilePath,
               DetectionReportType.Skipped);
        }

        [TestMethod]
        public void RmTest()
        {
            BaseDetectorTest(
               new ScannerRule("RmTest", @"rm -rf %userprofile%\Documents"),
               TestsEnviroment.RmMalvareFilePath,
               DetectionReportType.Malvare);
        }

        [TestMethod]
        public void FileNotExistsTest()
        {
            BaseDetectorTest(
                new ScannerRule("test", "test"),
                "NOSUCHFILE.NOFILE",
                DetectionReportType.FileNotExists);
        }

        private void BaseDetectorTest(ScannerRule rule, string filePath, DetectionReportType expected)
        {
            var detector = new Detector(new ScannerRule[] { rule });
            var result = detector.CheckFile(filePath);

            Assert.AreEqual(expected, result.ReportType);
        }
    }
}
