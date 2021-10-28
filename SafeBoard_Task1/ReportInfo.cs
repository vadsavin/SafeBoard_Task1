using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SafeBoard_Task1
{
    /// <summary>
    /// Отвечает за хранение и доступ к результатм сканирования.
    /// </summary>
    public class ReportInfo
    {
        private ConcurrentBag<DetectionReport> _reports;

        private DateTime? _startTime;
        private DateTime? _endTime;

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

        public ReportInfo()
        {
            _reports = new();
        }

        /// <summary>
        /// Добавляет отчёт о сканировании в сумку.
        /// </summary>
        /// <param name="report">Отчёт о сканировании.</param>
        public void AddReport(DetectionReport report)
        {
            _reports.Add(report);
        }

        /// <summary>
        /// Поиск отчётов среди всех по определенному типу.  
        /// </summary>
        /// <param name="type">Тип отчёта.</param>
        /// <returns>Enumerable отчётов с указанным типом.</returns>
        public IEnumerable<DetectionReport> GetReportsByType(DetectionReportType type)
        {
            return _reports.Where(report => report.ReportType == type);
        }

        /// <summary>
        /// Получть доуступ ко всем отчётам.
        /// </summary>
        /// <returns>Массив отчётов.</returns>
        public DetectionReport[] GetAllReports()
        {
            return _reports.ToArray();
        }

        /// <summary>
        /// Посчитать количество отчётов.
        /// </summary>
        /// <returns>Количество отчётов.</returns>
        public int GetAmountOfReports()
        {
            return _reports.Count;
        }

        public void StartScanning()
        {
            _startTime = DateTime.Now;
        }

        public void EndScanning()
        {
            _endTime = DateTime.Now;
        }
    }
}
