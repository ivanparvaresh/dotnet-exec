using System;

namespace Ivanize.DotnetTool.Exec
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                // Services
                var fileSystem = new FileSystem();
                var defaultEntrypointDetector = new DefaultEntrypointDetector();
                var parser = new ScriptsFileParser(defaultEntrypointDetector);
                var configFileResolver = new ConfigFileResolver(fileSystem);
                var executor = new Executor();


                var app = new Application(
                    configFileResolver,
                    parser,
                    executor);
                return app.Run(args);
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}
