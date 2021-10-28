using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace SafeBoard_Task1
{
    public class Scanner
    {
        public ScannerRule[] Rules { get; set; }

        public ReportInfo ReportInfo { get; private set; }

        public Scanner()
        {
            ReportInfo = new ReportInfo();
        }

        public Scanner(ScannerRule[] rules)
            :this()
        {
            Rules = rules;
        }

        public void Scan(string path)
        {
            ReportInfo = new ReportInfo();
            ReportInfo.StartScanning();

            string[] allFilesPath = GetAllFilesFromDirectory(path);

            Detector detector = new Detector(Rules);

            Parallel.ForEach(allFilesPath, fileName =>
            {
                var report = detector.CheckFile(fileName);
                ReportInfo.AddReport(report);
            });

            ReportInfo.EndScanning();
        }

        public string[] GetAllFilesFromDirectory(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        }
    }
}
