using System.IO;

namespace Ivanize.DotnetTool.Exec
{
    public interface IScriptsFileParser
    {
        Package Parse(StreamReader scriptsFileStream);
    }
}
