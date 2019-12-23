using System.IO;

namespace Ivanize.DotnetTool.Exec
{
    public class FileSystem : IFileSystem
    {
        public FileSystem()
        {
        }

        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }
        public StreamReader OpenText(string fileName)
        {
            return File.OpenText(fileName);
        }
    }
}
