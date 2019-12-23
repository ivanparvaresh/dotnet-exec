using System;

namespace Ivanize.DotnetTool.Exec
{
    public class Command
    {
        public string Name { get; private set; }
        public string[] Scripts { get; private set; }

        public Command(string name, string[] scripts)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Scripts = scripts ?? throw new ArgumentNullException(nameof(scripts));
        }
    }
}
