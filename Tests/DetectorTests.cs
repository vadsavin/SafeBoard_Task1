using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeBoard_Task1;
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
            var detector = new Detector(new ScannerRule[] { new ScannerRule("testClean", "test") });
            var result = detector.CheckFile(TestsEnviroment.CleanFilePath);

            Assert.AreEqual(DetectionReportType.Clean, result.ReportType);
        }

        [TestMethod]
        public void JsMalvareTest()
        {
            var detector = new Detector(new ScannerRule[] { new ScannerRule("testJsMalvare", @".*\.js", @"<script>evil_script()</script>") });
            var result = detector.CheckFile(TestsEnviroment.JsMalvareFilePath);

            Assert.AreEqual(DetectionReportType.Malvare, result.ReportType);
        }

        [TestMethod]
        public void JsMalvareCleanTest()
        {
            var detector = new Detector(new ScannerRule[] { new ScannerRule("testJsCleanMalvare", @".*\.js", "<script>evil_script()</script>") });
            var result = detector.CheckFile(TestsEnviroment.JsMalvareCleanFilePath);

            Assert.AreEqual(DetectionReportType.Clean, result.ReportType);
        }

        [TestMethod]
        public void RmTest()
        {
            var detector = new Detector(new ScannerRule[] { new ScannerRule("RmTest", @"rm -rf %userprofile%\Documents") });
            var result = detector.CheckFile(TestsEnviroment.RmMalvareFilePath);

            Assert.AreEqual(DetectionReportType.Malvare,result.ReportType);
        }

        [TestMethod]
        public void FileNotExistsTest()
        {
            var detector = new Detector(new ScannerRule[] { new ScannerRule("test", "test") });
            var result = detector.CheckFile("NOSUCHFILE.NOFILE");

            Assert.AreEqual(DetectionReportType.FileNotExists, result.ReportType);
        }
    }
}
