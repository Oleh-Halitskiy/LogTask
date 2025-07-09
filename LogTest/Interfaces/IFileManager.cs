namespace LogTest.Interfaces
{
    public interface IFileManager
    {
        void CreateFile(string directoryPath, string fileName);
        void AppendLine(string path, string content);
    }
}
