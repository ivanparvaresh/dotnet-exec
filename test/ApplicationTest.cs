using System;
using System.IO;
using Xunit;

namespace Ivanize.DotnetTool.Exec.Test
{
    public class ApplicationTest
    {

        [Fact]
        public void Should_Raise_If_File_Not_Found()
        {
            var fileSystem = new FakeFileSystem("dummy", "");
            var resolver = new ConfigFileResolver(fileSystem);
            var parser = new ScriptsFileParser(new DefaultEntrypointDetector());
            var executor = new Executor();
            var app = new Application(fileSystem, resolver, parser, executor);
            Assert.Throws<FileNotFoundException>(() => app.Run(new string[] { }));
        }
        [Fact]
        public void Should_Raise_If_Args_Null()
        {
            var fileSystem = new FakeFileSystem("./.dotnetexec.json", "Data");
            var resolver = new ConfigFileResolver(fileSystem);
            var parser = new ScriptsFileParser(new DefaultEntrypointDetector());
            var executor = new Executor();
            var app = new Application(fileSystem, resolver, parser, executor);
            Assert.Throws<ArgumentNullException>(() => app.Run(null));
        }

        [Fact]
        public void Should_Return_Executed_Method_Result()
        {
            var fileSystem = new FakeFileSystem("./.dotnetexec.json", "{ 'name':'MyTest' }");
            var resolver = new ConfigFileResolver(fileSystem);
            var parser = new ScriptsFileParser(new DefaultEntrypointDetector());
            var executor = new FakeExecutor();
            var app = new Application(fileSystem, resolver, parser, executor);
            Assert.Equal(-10, app.Run(new string[] { }));
        }
        [Fact]
        public void Should_Create_new_Config_File()
        {
            var defaultEntrypointDetector = new MockedDefaultEntrypointDetector();
            var fileSystem = new FakeFileSystem("--", "{ 'name':'MyTest' }");
            var resolver = new ConfigFileResolver(fileSystem);
            var parser = new ScriptsFileParser(defaultEntrypointDetector);
            var executor = new FakeExecutor();
            var app = new Application(fileSystem, resolver, parser, executor);

            var result = app.Run(new string[] { "config-init" });

            Assert.Equal(0, result);
            Assert.Equal(fileSystem.fileName, "./.dotnetexec.json");

            // should be parsable
            var stream = fileSystem.OpenText("./.dotnetexec.json");
            var pkg = parser.Parse(stream);
            Assert.Equal("app", pkg.Name);
            Assert.Equal("/bin/bash", pkg.Entrypoint);
            Assert.Equal(0, (int)pkg.Variables.Length);
            Assert.Equal(2, (int)pkg.Commands.Length);
            Assert.Equal("build", pkg.Commands[0].Name);
            Assert.Equal("dotnet build", pkg.Commands[0].Scripts[0]);
            Assert.Equal("test", pkg.Commands[1].Name);
            Assert.Equal("dotnet test", pkg.Commands[1].Scripts[0]);
        }

        // internal classes
        private class FakeExecutor : IExecutor
        {
            public int Execute(Package package, string[] args)
            {
                return -10;
            }
        }
        private class FakeFileSystem : IFileSystem
        {
            public string fileName;
            public string data;


            public FakeFileSystem(string fileName, string data)
            {
                this.fileName = fileName;
                this.data = data;
            }

            public bool Exists(string fileName)
            {
                return this.fileName == fileName;
            }

            public StreamReader OpenText(string fileName)
            {
                if (this.fileName == fileName)
                    return new StreamReader(new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(this.data)));

                throw new FileNotFoundException("FileNotFound", this.fileName);
            }
            public void WriteText(string fileName, string text)
            {
                this.fileName = fileName;
                this.data = text;
            }
        }
        // Internal Mocking Classes
        private class MockedDefaultEntrypointDetector : IDefaultEntrypointDetector
        {
            public string GetDefaultEntryPoint() => "/bin/bash";
        }
    }
}
