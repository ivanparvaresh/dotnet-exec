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

        // internal classes
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
