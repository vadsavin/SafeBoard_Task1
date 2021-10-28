using SafeBoard_Task1.Contacts;
using System;
using System.IO;
using System.Linq;

namespace SafeBoard_Task1
{
    /// <summary>
    /// Отвечает за оптимизирование чтение файла и анализ его содержимого.
    /// </summary>
    public class Detector
    {
        public ScannerRule[] Rules { get; }

        /// <summary>
        /// Длинна буфера для чтения (байт).
        /// </summary>
        public int BufferLength { get; set; } = 1024 * 1024 * 1;

        public Detector(ScannerRule[] rules)
        {
            Rules = rules;
        }

        /// <summary>
        /// Проверяет файл на соответствие правилам.
        /// </summary>
        public DetectionReport CheckFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new DetectionReport(filePath, DetectionReportType.FileNotExists);
                }

                return ScanFile(filePath);
            }
            catch (UnauthorizedAccessException)
            {
                return new DetectionReport(filePath, DetectionReportType.NoAccess);
            }
            catch (Exception e)
            {
                return new DetectionReport(filePath, DetectionReportType.Error, e.Message);
            }
        }

        /// <summary>
        /// Сканирует файл на наличие вредоносного ПО. Сканирование выполеняется последовательно блоками, что оптимизирует использование памяти.
        /// </summary>
        private DetectionReport ScanFile(string filePath)
        {
            int allBytesRead = 0;

            //Отсечение правил, которые не нужны для обработки файла.
            var rulesToScan = Rules
                .Where(rule => rule.CheckFileName(Path.GetFileName(filePath)))
                .Select(rule => new BlockScanInfo(rule))
                .ToArray();

            if (rulesToScan.Length == 0)
            {
                return new DetectionReport(filePath, DetectionReportType.Skipped, allBytesRead);
            }

            using (var file = File.OpenRead(filePath))
            using (var streamReader = new StreamReader(file))
            {
                int offset = 0;
                long length = streamReader.BaseStream.Length;
                char[] buffer = new char[BufferLength];

                //Последовательное чтение фрагментов файла и поиск в них соответсий правилам.
                int bytesRead = 0;
                while ((bytesRead = streamReader.Read(buffer, offset, BufferLength)) > 0)
                {
                    allBytesRead += bytesRead;

                    var result = ScanBlock(buffer, bytesRead, rulesToScan);
                    if (result != null)
                    {
                        return GenerateReport(filePath, result, allBytesRead);
                    }
                }
            }

            return new DetectionReport(filePath, DetectionReportType.Clean, allBytesRead);
        }

        /// <summary>
        /// Сканирует блок байт на соответствие правилам. Реализаация предусматривает использование предыдщих результатов.
        /// </summary>
        private BlockScanInfo ScanBlock(char[] buffer, int lenght, BlockScanInfo[] lastResult)
        {
            for (int i = 0; i < lenght; i++)
            {
                char symbol = buffer[i];

                foreach (var result in lastResult)
                {
                    if (result.GetCurrentSymbol() == symbol)
                    {
                        if (++result.Offset == result.Rule.MalvareString.Length)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        result.Offset = 0;
                    }
                }
                
            }

            return null;
        }

        private DetectionReport GenerateReport(string filePath, BlockScanInfo info, long bytesRead)
        {
            return new DetectionReport(filePath, DetectionReportType.Malvare, info.Rule, bytesRead);
        }
    }
}
