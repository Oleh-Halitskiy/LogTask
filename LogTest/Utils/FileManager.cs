using LogTest.Interfaces;
using System;
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
            File.Create(filePath).Close();
        }
        
        public void AppendLine(string path, string content)
        {
            if (File.Exists(path))
            {
                File.AppendAllText(path, content + Environment.NewLine);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
}
