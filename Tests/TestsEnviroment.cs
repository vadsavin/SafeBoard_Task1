using System.IO;

namespace Tests
{
    public class TestsEnviroment
    {
        public static string AffectedDirectory => Path.Combine(Directory.GetCurrentDirectory(), "AffectedFiles");
        public static string CleanFilePath => Path.Combine(AffectedDirectory, "clean.txt");
        public static string JsCleanFilePath => Path.Combine(AffectedDirectory, "js_clean.js");
        public static string JsMalvareCleanFilePath => Path.Combine(AffectedDirectory, "js_malvare.clean");
        public static string JsMalvareFilePath => Path.Combine(AffectedDirectory, "js_malvare.js");
        public static string RmMalvareFilePath => Path.Combine(AffectedDirectory, "rm -rf malvare.sh");
        public static string RundllFilePath => Path.Combine(AffectedDirectory, "Rundll32.bat");
    }
}
