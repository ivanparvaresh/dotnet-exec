using System;

namespace Ivanize.DotnetTool.Exec
{
    public class Command
    {
        public string Name { get; private set; }
        public string Script { get; private set; }

        public Command(string name, string script)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Script = script ?? throw new ArgumentNullException(nameof(script));
        }
    }
}
