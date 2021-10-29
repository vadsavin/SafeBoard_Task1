using SafeBoard_Task1.Contacts;
using System.Collections.Generic;

namespace Tests
{
    public class ReportDescription
    {
        public Dictionary<DetectionReportType, int> Reports { get; set; }
        public Dictionary<string, int> Rules { get; set; }
    }
}
