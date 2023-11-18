namespace LogTest.Interfaces
{
    /// <summary>
    /// Utility class that will allow us to manage files
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Creates file in specified directory, if directory doesn't exist it will create directory
        /// </summary>
        /// <param name="directoryPath">Directory where to create file</param>
        /// <param name="fileName">File name to create</param>
        void CreateFile(string directoryPath, string fileName);

        /// <summary>
        /// Writes content to file in specified path
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="content">Content to write</param>
        void WriteToFile(string path, string content);
    }
}
