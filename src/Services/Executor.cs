using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;

namespace Ivanize.DotnetTool.Exec
{
    public class Executor : IExecutor
    {
        public TextWriter OutWriter { get; private set; }
        public TextWriter ErrorWriter { get; private set; }

        public Executor() : this(Console.Out, Console.Error)
        {
        }
        public Executor(TextWriter outWriter, TextWriter errorWriter)
        {
            this.OutWriter = outWriter ?? throw new ArgumentNullException(nameof(outWriter));
            this.ErrorWriter = errorWriter ?? throw new ArgumentNullException(nameof(errorWriter));
        }

        public int Execute(Package package, string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            if (package == null) throw new ArgumentNullException(nameof(package));

            var app = new CommandLineApplication(false);
            app.Out = this.OutWriter;
            app.Error = this.ErrorWriter;
            app.HelpOption("-h");

            foreach (var command in package.Commands)
            {
                app.Command(command.Name, cmd =>
                {
                    cmd.Description = $" Run {command.Name} command";
                    cmd.OnExecute(() =>
                    {
                        return this.executeScript(package, command.Scripts);
                    });
                });
            }
            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });
            return app.Execute(args);
        }

        private int executeScript(Package package, string[] scripts)
        {
            var scriptText = new System.Text.StringBuilder();
            foreach (var script in scripts)
            {
                if (scriptText.Length != 0)
                    scriptText.Append(" && ");

                var escapedArgs = script.Replace("\"", "\\\"");
                scriptText.Append(escapedArgs);
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = package.Entrypoint,
                    Arguments = $"-c \"{scriptText.ToString()}\"",
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
            foreach (var env in package.Variables)
                process.StartInfo.EnvironmentVariables[env.Name] = env.Value;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
