using LogTest.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace LogTest.Utils
{
    public class FileManager : IFileManager
    {
        public void CreateFile(string directoryPath, string fileName)
        {
            string filePath = Path.Combine(directoryPath, fileName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            using (File.Create(filePath)) { }
        }

        public void WriteToFile(string path, string content)
        {
            if (File.Exists(path))
            {
                var contentList = new List<string>();
                contentList.Add(content);
                File.AppendAllLines(path, contentList);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
}
