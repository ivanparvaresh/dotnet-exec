using System;

namespace Ivanize.DotnetTool.Exec
{
    public class Application : IApplication
    {
        const string FILENAME = "./.dotnetexec.json";

        private IConfigFileResolver configFileResolver;
        private IScriptsFileParser parser;
        private IExecutor executor;
        private IFileSystem fileSystem;

        public Application(
            IFileSystem fileSystem,
            IConfigFileResolver configFileResolver,
            IScriptsFileParser parser,
            IExecutor executor)
        {
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            this.configFileResolver = configFileResolver ?? throw new ArgumentNullException(nameof(configFileResolver));
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        public int Run(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            if (args.Length > 0 && args[0] == "config-init")
            {
                fileSystem.WriteText(FILENAME, "{\n\t\"name\":\"app\",\n\t\"env\":{},\n\t\"commands\":{ \n\t\t\"build\":[\"dotnet build\"], \n\t\t\"test\":[\"dotnet test\"] \n\t}\n}");
                System.Console.WriteLine($"Configuratil file created: '{FILENAME}'");
                return 0;
            }


            if (!this.configFileResolver.Exists(FILENAME))
                throw new System.IO.FileNotFoundException($"No '{FILENAME}' config file found at {System.IO.Directory.GetCurrentDirectory()}. Use dotnet execute config-init to create configuration file.");

            var package = this.parser.Parse(this.configFileResolver.OpenText(FILENAME));
            return this.executor.Execute(package, args);
        }
    }
}
