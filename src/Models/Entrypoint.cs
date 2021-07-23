namespace Ivanize.DotnetTool.Exec
{
  public class Entrypoint
  {
    public Entrypoint(string executable, string commandOption)
    {
      Executable = executable;
      CommandOption = commandOption;
    }

    public string Executable { get; }
    public string CommandOption { get; }

    public static Entrypoint Windows { get; } = new Entrypoint("cmd.exe", "/c");
    public static Entrypoint Unix { get; } = new Entrypoint("/bin/bash", "-c");
  }
}