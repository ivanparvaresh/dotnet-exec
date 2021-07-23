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

    public string GetCurrentDirectory()
    {
      return Directory.GetCurrentDirectory();
    }

    public string GetParentDirectoryPath(string path)
    {
      var parent = Directory.GetParent(path);
      if (parent == null) return null;
      return parent.FullName;
    }

    public StreamReader OpenText(string fileName)
    {
      return File.OpenText(fileName);
    }

    public void WriteText(string fileName, string text)
    {
      File.WriteAllText(fileName, text);
    }
  }
}
