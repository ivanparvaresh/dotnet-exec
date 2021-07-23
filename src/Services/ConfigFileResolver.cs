using System;
using System.IO;

namespace Ivanize.DotnetTool.Exec
{
  public class ConfigFileResolver : IConfigFileResolver
  {

    private IFileSystem fileSystem;

    public string ResolveConfigFilePath(string fileName)
    {
      var currentDirectory = this.fileSystem.GetCurrentDirectory();
      while (true)
      {
        var expectedFilePath = Path.Combine(currentDirectory, fileName);
        if (this.Exists(expectedFilePath)) return expectedFilePath;
        var parentDir = this.fileSystem.GetParentDirectoryPath(currentDirectory);
        if (parentDir == null) throw new System.IO.FileNotFoundException($"Unable to locate dotnet-exec configuration file. Use 'dotnet execute config-init' to create configuration file.");
        currentDirectory = parentDir;
      }
    }

    public ConfigFileResolver(IFileSystem fileSystem)
    {
      this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public bool Exists(string fileName)
    {
      return this.fileSystem.Exists(fileName);
    }
    public StreamReader OpenText(string fileName)
    {
      return this.fileSystem.OpenText(fileName);
    }
  }
}
