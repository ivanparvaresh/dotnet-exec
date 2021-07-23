using System.IO;

namespace Ivanize.DotnetTool.Exec
{
  public interface IFileSystem
  {
    bool Exists(string fileName);
    StreamReader OpenText(string fileName);
    void WriteText(string fileName, string text);
    string GetCurrentDirectory();
    string GetParentDirectoryPath(string path);
  }
}
