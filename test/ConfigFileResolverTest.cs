using System.IO;
using Xunit;

namespace Ivanize.DotnetTool.Exec.Test
{
  public class ConfigFileResolverTest
  {
    [Fact]
    public void Should_Exists_Should_Return_False_If_File_Not_Found()
    {
      var fileSystem = new FakeFileSystem("dummy", "");
      var resolver = new ConfigFileResolver(fileSystem);
      Assert.False(resolver.Exists(".dotnetexec.json"));
    }

    [Fact]
    public void Should_Exists_Should_Return_True_If_File_Exists()
    {
      var fileSystem = new FakeFileSystem("dotnetexec.json", "");
      var resolver = new ConfigFileResolver(fileSystem);
      Assert.False(resolver.Exists(".dotnetexec.json"));
    }
    [Fact]
    public void Should_Exists_Should_Return_Content()
    {
      var fileSystem = new FakeFileSystem(".dotnetexec.json", "MyData");
      var resolver = new ConfigFileResolver(fileSystem);
      var reader = resolver.OpenText(".dotnetexec.json");
      var data = reader.ReadToEnd();
      Assert.Equal("MyData", data);
    }

    [Fact]
    public void Should_Return_FilePath_If_Exists_In_Current_Directory()
    {
      var fileSystem = new FakeFileSystem("/app/project/.dotnetexec.json", "MyData", "/app/project/");
      var resolver = new ConfigFileResolver(fileSystem);
      var fileName = resolver.ResolveConfigFilePath(".dotnetexec.json");
      Assert.Equal("/app/project/.dotnetexec.json", fileName);
    }
    [Fact]
    public void Should_Return_FilePath_If_Exists_In_Parent_Directory()
    {
      var fileSystem = new FakeFileSystem("/app/.dotnetexec.json", "MyData", "/app/project");
      var resolver = new ConfigFileResolver(fileSystem);
      var fileName = resolver.ResolveConfigFilePath(".dotnetexec.json");
      Assert.Equal("/app/.dotnetexec.json", fileName);
    }
    [Fact]
    public void Should_Throw_Exception_If_Config_File_Not_Found()
    {
      var fileSystem = new FakeFileSystem("/app/.soem-other-file.json", "MyData", "/app/project");
      var resolver = new ConfigFileResolver(fileSystem);
      Assert.ThrowsAny<FileNotFoundException>(() => resolver.ResolveConfigFilePath(".dotnetexec.json"));
    }

    // internal classes
    private class FakeFileSystem : IFileSystem
    {
      private string fileName;
      private string data;


      public FakeFileSystem(string fileName = "/app/project1/.dotnetexec.json", string data = "", string currentDirectory = "/app/project")
      {
        this.fileName = fileName;
        this.data = data;
      }

      public bool Exists(string fileName)
      {
        return this.fileName == fileName;
      }

      public string GetCurrentDirectory()
      {
        return "/app/project";
      }

      public string GetParentDirectoryPath(string path)
      {
        if (path == "/app/project") return "/app";
        if (path == "/app") return "/";
        return null;
      }

      public StreamReader OpenText(string fileName)
      {
        if (this.fileName == fileName)
          return new StreamReader(new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(this.data)));

        throw new FileNotFoundException("FileNotFound", this.fileName);
      }

      public void WriteText(string fileName, string text)
      {
        throw new System.NotImplementedException();
      }

    }
  }
}
