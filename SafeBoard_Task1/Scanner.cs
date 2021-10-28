using SafeBoard_Task1.Contacts;
using System.Threading.Tasks;

namespace SafeBoard_Task1
{
    public class Scanner
    {
        public ScannerRule[] Rules { get; }

        public ReportInfo ReportInfo => _reportInfo ?? new ReportInfo();

        public int MaxParallelScanningFiles { get; set; } = 10;

        private ReportInfo _reportInfo;

        public Scanner(ScannerRule[] rules)
        {
            Rules = rules;
        }

        public void Scan(string path)
        {
            ScanAsync(path).Wait();
        }

        /// <summary>
        /// Запуск работы сканера. Parallel используется для увелечения скорости работы. 
        /// </summary>
        public async Task ScanAsync(string path)
        {
            try
            {
                _reportInfo = new ReportInfo(Rules);
                _reportInfo.StartScanning();

                await Task.Run(() => RunScanJob(path));
            }
            finally
            {
                _reportInfo.EndScanning();
            }
        }

        private void RunScanJob(string path)
        {
            var parallelOptions = new ParallelOptions() 
            { 
                MaxDegreeOfParallelism = MaxParallelScanningFiles 
            };

            var filesEnumerator = FilesManager.GetAllFilesFromDirectory(path);

            Detector detector = new Detector(Rules);

            Parallel.ForEach(filesEnumerator, parallelOptions, filePath => ScanSingleFile(detector, filePath));
        }

        private void ScanSingleFile(Detector detector, string filePath)
        {
            var report = detector.CheckFile(filePath);

            _reportInfo.AddReport(report);
        }
    }
}
