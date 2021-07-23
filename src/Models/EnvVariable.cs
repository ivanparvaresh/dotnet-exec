using System;

namespace Ivanize.DotnetTool.Exec
{
  public class EnvVariable
  {
    public string Name { get; private set; }
    public string Value { get; private set; }

    public EnvVariable(string name, string value)
    {
      this.Name = name ?? throw new ArgumentNullException(nameof(name));
      this.Value = value ?? throw new ArgumentNullException(nameof(value));
    }
  }
}
