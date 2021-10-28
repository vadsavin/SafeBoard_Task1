using SafeBoard_Task1.Contacts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SafeBoard_Task1
{
    public class Scanner
    {
        public ScannerRule[] Rules { get; }

        public ReportInfo ReportInfo => _reportInfo ?? new ReportInfo();

        public int MaxParallelScanningFiles { get; set; } = 10;

        public bool LogSkippedFiles { get; set; } = true;

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
                _reportInfo = new ReportInfo();
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

            var filesEnumerator = GetAllFilesFromDirectory(path);

            Detector detector = new Detector(Rules);

            Parallel.ForEach(filesEnumerator, parallelOptions, filePath => ScanSingleFile(detector, filePath));
        }

        private void ScanSingleFile(Detector detector, string filePath)
        {
            var report = detector.CheckFile(filePath);

            if (report.ReportType == DetectionReportType.Skipped && !LogSkippedFiles)
            {
                return;
            }

            _reportInfo.AddReport(report);
        }

        private IEnumerable<string> GetAllFilesFromDirectory(string path)
        {
            var directories = new List<string>() { path };

            while (directories.Any())
            {
                var tmp = directories.ToArray();
                directories.Clear();

                foreach (var directory in tmp)
                {
                    foreach (var filePath in GetDirectoryEntries(directory, directories))
                    {
                        yield return filePath;
                    }
                }
            }
        }

        private IEnumerable<string> GetDirectoryEntries(string path, List<string> directories)
        {
            try
            {
                directories.AddRange(Directory.GetDirectories(path));
                return Directory.GetFiles(path);
            }
            catch
            {
                return new string[0];
            }
        }
    }
}
