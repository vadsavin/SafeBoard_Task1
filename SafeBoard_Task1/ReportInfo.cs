using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SafeBoard_Task1
{
    public class ReportInfo
    {
        private ConcurrentBag<DetectionReport> _reports;

        private DateTime? _startTime;
        private DateTime? _endTime;

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

        public void AddReport(DetectionReport report)
        {
            _reports.Add(report);
        }

        public IEnumerable<DetectionReport> GetReportsByType(DetectionReportType type)
        {
            return _reports.Where(report => report.ReportType == type);
        }

        public DetectionReport[] GetAllReports()
        {
            return _reports.ToArray();
        }

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
