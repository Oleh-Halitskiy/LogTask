using LogTest.Utils;
using System.IO;
using Xunit;

namespace LogComponent.Tests
{
    public class FileManagerTests
    {
        [Fact]
        public void CreateFile_ShouldCreateFile()
        {
            var fileManager = new FileManager();
            var directoryPath = Path.Combine(Path.GetTempPath(), "TestDirectory");
            var fileName = "TestFile.txt";
            var filePath = Path.Combine(directoryPath, fileName);

            try
            {
                fileManager.CreateFile(directoryPath, fileName);

                Assert.True(File.Exists(filePath));
            }
            finally
            {
                Directory.Delete(directoryPath, true);
            }
        }

        [Fact]
        public void WriteToFile_ShouldWriteContentToFile()
        {
            var fileManager = new FileManager();
            var directoryPath = Path.Combine(Path.GetTempPath(), "TestDirectory");
            var fileName = "TestFile.txt";
            var filePath = Path.Combine(directoryPath, fileName);
            var content = "Hello, world!";

            try
            {
                fileManager.CreateFile(directoryPath, fileName);
                fileManager.WriteToFile(filePath, content);

                var fileContent = File.ReadAllText(filePath);
                Assert.Contains(content, fileContent);
            }
            finally
            {
                Directory.Delete(directoryPath, true);
            }
        }

        [Fact]
        public void WriteToFile_ShouldThrowDirectoryNotFoundException()
        {
            var fileManager = new FileManager();
            var nonExistentPath = Path.Combine(Path.GetTempPath(), "NonExistentDirectory");
            var fileName = "NonExistentFile.txt";
            var filePath = Path.Combine(nonExistentPath, fileName);
            var content = "Hello, world!";

            Assert.Throws<DirectoryNotFoundException>(() => fileManager.WriteToFile(filePath, content));
        }
    }
}
