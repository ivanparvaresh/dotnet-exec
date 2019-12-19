namespace Ivanize.DotnetTool.Exec
{
    class Program
    {
        const string FILENAME = "./.dotnetexec.json";
        static void Main(string[] args)
        {
            var defaultEntrypointDetector = new DefaultEntrypointDetector();
            var parser = new ScriptsFileParser(defaultEntrypointDetector);
            if (!System.IO.File.Exists(FILENAME))
                throw new System.IO.FileNotFoundException();

            var package = parser.Parse(System.IO.File.OpenText(FILENAME));

            var executor = new Executor(package);
            executor.Execute(args);
        }
    }
}
