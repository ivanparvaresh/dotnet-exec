namespace Ivanize.DotnetTool.Exec
{
  public interface IExecutor
  {
    int Execute(Package package, string[] args);
  }
}
