namespace SafeBoard_Task1
{
    public class DetectionReport
    {
        public DetectionReportType ReportType { get; }
        public string Message { get; }
        public ScannerRule Rule { get; }

        public DetectionReport(DetectionReportType type)
        {
            ReportType = type;
        }

        public DetectionReport(DetectionReportType type, ScannerRule rule)
            : this(type)
        {
            Rule = rule;
        }

        public DetectionReport(DetectionReportType type, string message)
            : this(type)
        {
            Message = message;
        }
    }
}
