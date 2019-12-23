using System.IO;

namespace Ivanize.DotnetTool.Exec
{
    public interface IConfigFileResolver
    {
        bool Exists(string fileName);
        StreamReader OpenText(string fileName);
    }
}
