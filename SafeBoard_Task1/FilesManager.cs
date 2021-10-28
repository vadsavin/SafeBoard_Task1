using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SafeBoard_Task1
{
    public sealed class FilesManager
    {
        public static IEnumerable<string> GetAllFilesFromDirectory(string path)
        {
            var directories = new List<string>() { path };

            while (directories.Any())
            {
                var tmp = directories.ToArray();
                directories.Clear();

                foreach (var directory in tmp)
                {
                    foreach (var filePath in GetDirectoryEntries(directory, directories))
                    {
                        yield return filePath;
                    }
                }
            }
        }

        private static IEnumerable<string> GetDirectoryEntries(string path, List<string> directories)
        {
            try
            {
                directories.AddRange(Directory.GetDirectories(path));
                return Directory.GetFiles(path);
            }
            catch
            {
                return new string[0];
            }
        }
    }
}
