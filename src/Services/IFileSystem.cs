using System.IO;

namespace Ivanize.DotnetTool.Exec
{
    public interface IFileSystem
    {
        bool Exists(string fileName);
        StreamReader OpenText(string fileName);
    }
}
