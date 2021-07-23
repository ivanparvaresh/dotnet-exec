using System;

namespace Ivanize.DotnetTool.Exec
{
  public class Package
  {
    public string Name { get; private set; }
    public string Entrypoint => EntrypointObject.Executable;
    public Entrypoint EntrypointObject { get; }
    public EnvVariable[] Variables { get; set; }
    public Command[] Commands { get; set; }

    public Package(string name, Entrypoint entrypoint, EnvVariable[] variables, Command[] commands)
    {
      this.Name = name ?? throw new ArgumentNullException(nameof(name));
      this.EntrypointObject = entrypoint ?? throw new ArgumentNullException(nameof(entrypoint));
      this.Variables = variables ?? throw new ArgumentNullException(nameof(variables));
      this.Commands = commands ?? throw new ArgumentNullException(nameof(commands));
    }


  }
}
