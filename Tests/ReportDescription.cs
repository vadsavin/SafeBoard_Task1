using SafeBoard_Task1.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ReportDescription
    {
        public Dictionary<DetectionReportType, int> Reports { get; set; }
        public Dictionary<string, int> Rules { get; set; }
    }
}
