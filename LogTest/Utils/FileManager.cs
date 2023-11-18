using LogTest.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace LogTest.Utils
{
    /// <summary>
    /// Utility class that will allow us to manage files
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <inheritdoc />
        public void CreateFile(string directoryPath, string fileName)
        {
            string filePath = Path.Combine(directoryPath, fileName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (File.Create(filePath)) { }
        }

        /// <inheritdoc />
        public void WriteToFile(string path, string content)
        {
            if (File.Exists(path))
            {
                var contentList = new List<string> { content };
                File.AppendAllLines(path, contentList);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
}
