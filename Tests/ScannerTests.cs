using Microsoft.VisualStudio.TestTools.UnitTesting;
using SafeBoard_Task1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        public void GetAllFilesFromDirectoryTest()
        {
            var scanner = new Scanner();

            string[] actulaFiles = scanner.GetAllFilesFromDirectory(TestsEnviroment.AffectedDirectory);
            string[] fileNames = new string[]
            {
                "clean.txt",
                "js_clean.js",
                "js_malvare.js",
                "js_malvare.clean",
                "rm-rfmalvare.sh",
                "Rundll32.bat"
            };
            string currentDirectory = Directory.GetCurrentDirectory();

            CollectionAssert.AreEquivalent(fileNames, actulaFiles.Select(file => Path.GetFileName(file)).ToArray());
        }   
    }
}
