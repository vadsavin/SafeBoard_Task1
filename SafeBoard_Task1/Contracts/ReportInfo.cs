using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SafeBoard_Task1.Contacts
{
    /// <summary>
    /// Отвечает за хранение и доступ к результатм сканирования.
    /// </summary>
    public class ReportInfo
    {
        private ConcurrentQueue<DetectionReport> _reports;

        private DateTime? _startTime;
        private DateTime? _endTime;
        private bool _scanInProgress = false;
        private long _bytesRead;

        /// <summary>
        /// Возвращает актуальное время сканирования
        /// </summary>
        public TimeSpan ScanningTime
        {
            get 
            {
                var currentTime = DateTime.Now;
                return (_endTime ?? currentTime) - (_startTime ?? currentTime);
            }
        }

        public bool ScanInProgress => _scanInProgress;

        public long BytesRead => _bytesRead;

        public ReportInfo()
        {
            _reports = new();
        }

        /// <summary>
        /// Добавляет отчёт о сканировании в сумку.
        /// </summary>
        public void AddReport(DetectionReport report)
        {
            _reports.Enqueue(report);
            IncrementRead(report.BytesRead);
        }

        /// <summary>
        /// Поиск отчётов среди всех по определенному типу.  
        /// </summary>
        public IEnumerable<DetectionReport> GetReportsByType(DetectionReportType type)
        {
            return _reports.Where(report => report.ReportType == type);
        }

        /// <summary>
        /// Получть доуступ ко всем отчётам.
        /// </summary>
        public IEnumerable<DetectionReport> GetAllReports()
        {
            return _reports;
        }

        /// <summary>
        /// Посчитать количество отчётов.
        /// </summary>
        public int GetAmountOfReports()
        {
            return _reports.Count;
        }

        public void StartScanning()
        {
            _startTime = DateTime.Now;
            _scanInProgress = true;
        }

        public void EndScanning()
        {
            _endTime = DateTime.Now;
            _scanInProgress = false;
        }

        private void IncrementRead(long bytesLength)
        {
            Interlocked.Add(ref _bytesRead, bytesLength);
        }
    }
}
