using System;
using System.IO;
using System.Linq;

namespace SafeBoard_Task1
{
    public class Detector
    {
        public ScannerRule[] Rules { get; }

        public int BufferLength { get; set; } = 1024;

        public Detector(ScannerRule[] rules)
        {
            Rules = rules;
        }

        public DetectionReport CheckFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new DetectionReport(DetectionReportType.FileNotExists);
                }

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

        private DetectionReport ScanFile(string filePath)
        {
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
