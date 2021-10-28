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
        public int BufferLength { get; set; } = 1024;

        public Detector(ScannerRule[] rules)
        {
            Rules = rules;
        }

        /// <summary>
        /// Проверяет файл на наличие вредоносного кода.
        /// </summary>
        /// <param name="filePath">Путь к файлую</param>
        /// <returns>Отчёт о результате сканирования.</returns>
        public DetectionReport CheckFile(string filePath)
        {
            try
            {
                //Если файла не существует, возвращаем соответстующий отчет (должно работать быстрее, чем отдельный catch для этой ошибки).
                if (!File.Exists(filePath))
                {
                    return new DetectionReport(DetectionReportType.FileNotExists);
                }

                //Иначе возвращаем результат сканирования.
                return ScanFile(filePath);
            }
            catch (UnauthorizedAccessException)
            {
                return new DetectionReport(DetectionReportType.NoAccess);
            }
            catch (Exception e)
            {
                return new DetectionReport(DetectionReportType.Error, e.Message);
            }
        }

        /// <summary>
        /// Сканирует файл на наличие вредоносного ПО. Сканирование выполеняется последовательно блоками, что оптимизирует использование памяти.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Отчёт о сканировании.</returns>
        private DetectionReport ScanFile(string filePath)
        {
            //Отсечение правил, которые не нужны для обработки файла.
            var rulesToScan = Rules
                .Where(rule => rule.CheckFileName(Path.GetFileName(filePath)))
                .Select(rule => new BlockScanInfo(rule))
                .ToArray();

            using (var file = File.OpenRead(filePath))
            using (var streamReader = new StreamReader(file))
            {
                int offset = 0;
                long length = streamReader.BaseStream.Length;
                char[] buffer = new char[BufferLength];

                //Последовательное чтение фрагментов файла и поиск в них соответсий правилам.
                int bytes = 0;
                while ((bytes = streamReader.Read(buffer, offset, BufferLength)) > 0)
                {
                    var result = ScanBlock(buffer, bytes, rulesToScan);
                    if (result != null)
                    {
                        return GenerateReport(result);
                    }
                }
                
            }

            return new DetectionReport(DetectionReportType.Clean);
        }

        /// <summary>
        /// Сканирует блок байт на соответствие правилам. Реализаация предусматривает использование предыдщих результатов.
        /// </summary>
        /// <param name="buffer">Массив байт для анализа</param>
        /// <param name="lenght">Количество байт для анализа.</param>
        /// <param name="lastResult">Предыдщий результат сканирования.</param>
        /// <returns>Результат сканирования блока.</returns>
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

        private DetectionReport GenerateReport(BlockScanInfo info)
        {
            return new DetectionReport(DetectionReportType.Malvare, info.Rule);
        }
    }
}
