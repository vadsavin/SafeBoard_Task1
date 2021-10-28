using System.Text.RegularExpressions;

namespace SafeBoard_Task1.Contacts
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

        /// <summary>
        /// Проверяет соответствие названия файла вышеуказанному паттерну.
        /// </summary>
        public bool CheckFileName(string fileName) 
        {
            return string.IsNullOrEmpty(FileNamePattern)
                || Regex.IsMatch(fileName, FileNamePattern);
        }
    }
}
