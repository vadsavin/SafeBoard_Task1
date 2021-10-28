using System.IO;
using System.Threading.Tasks;

namespace SafeBoard_Task1
{
    public class Scanner
    {
        public ScannerRule[] Rules { get; }

        public ReportInfo ReportInfo { get; private set; }

        public Scanner(ScannerRule[] rules)
        {
            Rules = rules;
            ReportInfo = new ReportInfo();
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

        private string[] GetAllFilesFromDirectory(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        }
    }
}
