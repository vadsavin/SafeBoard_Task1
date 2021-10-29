using SafeBoard_Task1.Contacts;
using System.IO;

namespace Tests
{
    public class TestsEnviroment
    {
        public static string AffectedDirectory      => Path.Combine(Directory.GetCurrentDirectory(), "AffectedFiles");

        public static string CleanFilePath          => Path.Combine(AffectedDirectory, "clean.txt");
        public static string JsCleanFilePath        => Path.Combine(AffectedDirectory, "js_clean.js");
        public static string JsMalvareCleanFilePath => Path.Combine(AffectedDirectory, "js_malvare.clean");
        public static string JsMalvareFilePath      => Path.Combine(AffectedDirectory, "js_malvare.js");
        public static string RmMalvareFilePath      => Path.Combine(AffectedDirectory, "rm-rfmalvare.sh");
        public static string Rundll32FilePath       => Path.Combine(AffectedDirectory, "Rundll32.bat");

        public static ScannerRule JsRule            => new ScannerRule("JS", @".*\.js\Z", "<script>evil_script()</script>");
        public static ScannerRule RmRule            => new ScannerRule("rm -rf", @"rm -rf %userprofile%\Documents");
        public static ScannerRule Rundll32Rule      => new ScannerRule("Rundll32", "Rundll32 sus.dll SusEntry");
    }
}
