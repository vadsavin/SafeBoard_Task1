namespace SafeBoard_Task1
{
    public class BlockScanInfo
    {
        public ScannerRule Rule { get; }

        /// <summary>
        /// Количество символов, которое уже совпало с правилом.
        /// </summary>
        public int Offset { get; set; }

        public BlockScanInfo(ScannerRule rule)
        {
            Rule = rule;
            Offset = 0;
        }

        /// <summary>
        /// Возвращает текущий символ для проверки со строкой вредоносного кода из правила.
        /// </summary>
        /// <returns></returns>
        public char GetCurrentSymbol()
        {
            return Rule.MalvareString[Offset];
        }
    }
}
