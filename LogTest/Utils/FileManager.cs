using LogTest.Interfaces;
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
            File.Create(filePath);
        }

        public void WriteToFile(string path, string content)
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, content);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
}
