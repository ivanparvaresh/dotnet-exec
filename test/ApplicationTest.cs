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
            var app = new Application(resolver, parser, executor);
            Assert.Throws<FileNotFoundException>(() => app.Run(new string[] { }));
        }
        [Fact]
        public void Should_Raise_If_Args_Null()
        {
            var fileSystem = new FakeFileSystem("./.dotnetexec.json", "Data");
            var resolver = new ConfigFileResolver(fileSystem);
            var parser = new ScriptsFileParser(new DefaultEntrypointDetector());
            var executor = new Executor();
            var app = new Application(resolver, parser, executor);
            Assert.Throws<ArgumentNullException>(() => app.Run(null));
        }
        
        [Fact]
        public void Should_Return_Executed_Method_Result()
        {
            var fileSystem = new FakeFileSystem("./.dotnetexec.json", "{ 'name':'MyTest' }");
            var resolver = new ConfigFileResolver(fileSystem);
            var parser = new ScriptsFileParser(new DefaultEntrypointDetector());
            var executor = new FakeExecutor();
            var app = new Application(resolver, parser, executor);
            Assert.Equal(-10, app.Run(new string[] { }));
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
            private string fileName;
            private string data;


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
        }
    }
}
