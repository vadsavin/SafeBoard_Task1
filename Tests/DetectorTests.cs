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
            var detector = new Detector(new ScannerRule[] { new ScannerRule("test", "test") });
            var result = detector.CheckFile(TestsEnviroment.CleanFilePath);

            Assert.AreEqual(result.ReportType, DetectionReportType.Clean);
        }

        [TestMethod]
        public void JsMalvareTest()
        {

        }

        [TestMethod]
        public void RmTest()
        {

        }

        [TestMethod]
        public void FileNotExistsTest()
        {
            var detector = new Detector(new ScannerRule[] { new ScannerRule("test", "test") });
            var result = detector.CheckFile("NOSUCHFILE.NOFILE");

            Assert.AreEqual(result.ReportType, DetectionReportType.FileNotExists);
        }
    }
}
