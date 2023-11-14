namespace LogTest.Interfaces
{
    public interface IFileManager
    {
        void CreateFile(string directoryPath, string fileName);
        void WriteToFile(string path, string content);
    }
}
