using System;
using System.Linq;
using System.IO;
using System.Text;
using Xunit;

namespace Ivanize.DotnetTool.Exec.Test
{
  public class ScriptsFileParserTest
  {
    [Fact]
    public void Parse_Should_Throw_If_Stream_Is_Null()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var parser = new ScriptsFileParser(defaultEntrypointDetector);
      Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
    }
    [Fact]
    public void Parse_Should_Throw_If_Stream_Content_Is_Not_Valid()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);
      Assert.Throws<InvalidDataException>(() => parser.Parse(reader));
    }
    [Fact]
    public void Parse_Should_Throw_If_PkgName_Not_Defined()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{  }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);
      Assert.Throws<InvalidDataException>(() => parser.Parse(reader));
    }
    [Fact]
    public void Parse_Should_Throw_If_PkgName_Is_Null()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'' }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);
      Assert.Throws<InvalidDataException>(() => parser.Parse(reader));
    }

    [Fact]
    public void Parse_Should_Return_Empty_Package()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'MyPackage' }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);
      var pkg = parser.Parse(reader);

      Assert.True(pkg.Variables.Length == 0);
      Assert.True(pkg.Commands.Length == 0);
    }

    [Fact]
    public void Parse_Should_Throw_Error_If_variable_name_is_empty_or_null()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'MyPackage' , 'env':{ '':'test'  } }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);


      Assert.Throws<InvalidDataException>(() => parser.Parse(reader));
    }

    [Fact]
    public void Parse_Should_Throw_Error_If_commands_name_is_empty_or_null()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'MyPackage' , 'commands':{ '':['test']  } }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);


      Assert.Throws<InvalidDataException>(() => parser.Parse(reader));
    }

    [Fact]
    public void Parse_Should_Return_Commands()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'MyPackage' , 'commands':{ 'Test':['dotnet test']  } }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);

      var pkg = parser.Parse(reader);
      Assert.True(pkg.Commands.Length == 1);
      Assert.Collection(pkg.Commands, s => Assert.True(s.Name == "Test" && s.Scripts[0] == "dotnet test"));
    }
    [Fact]
    public void Parse_Should_Return_EnvVariables()
    {
      var defaultEntrypointDetector = new DefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'MyPackage' , 'env':{ 'TEST_ENV':'TEST_VALUE'  } }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);

      var pkg = parser.Parse(reader);
      Assert.True(pkg.Variables.Length == 1);
      Assert.Collection(pkg.Variables, s => Assert.True(s.Name == "TEST_ENV" && s.Value == "TEST_VALUE"));
    }

    [Fact]
    public void Parse_Should_Set_Entrypoint_default_If_It_Was_Default()
    {
      var defaultEntrypointDetector = new MockedDefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'MyPackage' }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);

      var pkg = parser.Parse(reader);
      Assert.True(pkg.Entrypoint == "/bin/bash");
    }
    [Fact]
    public void Parse_Should_Set_Entrypoint_If_It_Has_defined()
    {
      var defaultEntrypointDetector = new MockedDefaultEntrypointDetector();
      var reader = new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes("{ 'name':'MyPackage', entrypoint:'/bin/sh' }")));
      var parser = new ScriptsFileParser(defaultEntrypointDetector);

      var pkg = parser.Parse(reader);
      Assert.True(pkg.Entrypoint == "/bin/sh");
    }

    // Internal Mocking Classes
    private class MockedDefaultEntrypointDetector : IDefaultEntrypointDetector
    {
      public Entrypoint GetDefaultEntrypoint() => Entrypoint.Unix;
    }
  }
}
