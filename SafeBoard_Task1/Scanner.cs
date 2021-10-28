using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace SafeBoard_Task1
{
    public class Scanner
    {
        //Массив правил для поиска вредоносного кода.
        public ScannerRule[] Rules { get; set; }

        //Оъект для хранения результатов сканирования.
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

        /// <summary>
        /// Запуск работы сканера. Parallel используется для увелечения скорости работы. 
        /// </summary>
        /// <param name="path">Директория для поиска.</param>
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

        /// <summary>
        /// Поиск всех файлов в указанной директории и ее поддиректориях
        /// </summary>
        /// <param name="path">Директория для поиска.</param>
        /// <returns>Массив с абсолютными путями к файлам.</returns>
        public string[] GetAllFilesFromDirectory(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        }
    }
}
