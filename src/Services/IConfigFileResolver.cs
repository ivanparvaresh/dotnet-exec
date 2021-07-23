using System.IO;

namespace Ivanize.DotnetTool.Exec
{
  public interface IConfigFileResolver
  {
    string ResolveConfigFilePath(string fileName);
    bool Exists(string fileName);
    StreamReader OpenText(string fileName);
  }
}
