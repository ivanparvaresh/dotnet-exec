using System;

namespace Ivanize.DotnetTool.Exec
{
    public class Application : IApplication
    {
        const string FILENAME = "./.dotnetexec.json";

        private IConfigFileResolver configFileResolver;
        private IScriptsFileParser parser;
        private IExecutor executor;

        public Application(
            IConfigFileResolver configFileResolver,
            IScriptsFileParser parser,
            IExecutor executor)
        {
            this.configFileResolver = configFileResolver ?? throw new ArgumentNullException(nameof(configFileResolver));
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }   

        public int Run(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            if (!this.configFileResolver.Exists(FILENAME))
                throw new System.IO.FileNotFoundException($"No '{FILENAME}' file found at {System.IO.Directory.GetCurrentDirectory()}");

            var package = this.parser.Parse(this.configFileResolver.OpenText(FILENAME));
            return this.executor.Execute(package, args);
        }
    }
}
