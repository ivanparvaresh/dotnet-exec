using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;


namespace Ivanize.DotnetTool.Exec
{
    public class Executor : IExecutor
    {
        public Package Package { get; private set; }
        public TextWriter OutWriter { get; private set; }
        public TextWriter ErrorWriter { get; private set; }

        public Executor(Package package) : this(package, Console.Out, Console.Error)
        {
        }
        public Executor(Package package, TextWriter outWriter, TextWriter errorWriter)
        {
            this.Package = package ?? throw new ArgumentNullException(nameof(package));
            this.OutWriter = outWriter ?? throw new ArgumentNullException(nameof(outWriter));
            this.ErrorWriter = errorWriter ?? throw new ArgumentNullException(nameof(errorWriter));
        }

        public void Execute(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            var app = new CommandLineApplication(false);
            app.Out = this.OutWriter;
            app.Error = this.ErrorWriter;
            app.HelpOption("-h");

            foreach (var command in Package.Commands)
            {
                app.Command(command.Name, cmd =>
                {
                    cmd.Description = $" Run {command.Name} command";
                    cmd.OnExecute(() =>
                    {
                        return this.executeScript(command.Script);
                    });
                });
            }
            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });
            app.Execute(args);
        }

        private int executeScript(string script)
        {
            var escapedArgs = script.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Package.Entrypoint,
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            // Handle the output
            process.OutputDataReceived += (sender, data) =>
            {
                this.OutWriter.WriteLine(data.Data);
                this.OutWriter.Flush();
            };
            process.ErrorDataReceived += (sender, data) =>
            {
                this.ErrorWriter.WriteLine(data.Data);
                this.ErrorWriter.Flush();
            };

            // Set environment variables
            foreach (var env in this.Package.Variables)
                process.StartInfo.EnvironmentVariables[env.Name] = env.Value;

            process.Start();
			process.BeginErrorReadLine();
			process.BeginOutputReadLine();
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
