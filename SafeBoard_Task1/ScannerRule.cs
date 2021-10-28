using System.Text.RegularExpressions;

namespace SafeBoard_Task1
{
    public class ScannerRule
    {
        //Название правила.
        public string RuleName { get; }
        //Паттерн регулярного выражения для анализа названия файла.
        public string FileNamePattern { get; }
        //Строка с вредоносным кодом.
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
        /// <param name="fileName"></param>
        /// <returns>True, если название соответствует паттерну, иначе False</returns>
        public bool CheckFileName(string fileName) 
        {
            return string.IsNullOrEmpty(FileNamePattern)
                || Regex.IsMatch(fileName, FileNamePattern);
        }
    }
}
