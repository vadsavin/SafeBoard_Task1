using System.Text.RegularExpressions;

namespace SafeBoard_Task1
{
    public class ScannerRule
    {
        public string RuleName { get; }
        public string FileNamePattern { get; }
        public string MalvareString { get; }

        public ScannerRule(string ruleName, string malvareString)
        {
            RuleName = ruleName;
            MalvareString = malvareString;
        }

        public ScannerRule(string ruleName, string fileNamePattern, string malvareString) 
            :  this(ruleName, malvareString)
        {
            FileNamePattern = fileNamePattern;
        }

        public bool CheckFileName(string fileName) 
        {
            return string.IsNullOrEmpty(FileNamePattern)
                || Regex.IsMatch(fileName, FileNamePattern);
        }
    }
}
