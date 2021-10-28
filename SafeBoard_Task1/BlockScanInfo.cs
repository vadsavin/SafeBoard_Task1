namespace SafeBoard_Task1
{
    public class BlockScanInfo
    {
        public ScannerRule Rule { get; }

        public int Offset { get; set; }

        public BlockScanInfo(ScannerRule rule)
        {
            Rule = rule;
            Offset = 0;
        }

        public char GetCurrentSymbol()
        {
            return Rule.MalvareString[Offset];
        }
    }
}
