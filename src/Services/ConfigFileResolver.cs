using System;
using System.IO;

namespace Ivanize.DotnetTool.Exec
{
    public class ConfigFileResolver : IConfigFileResolver
    {
        private IFileSystem fileSystem;

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
