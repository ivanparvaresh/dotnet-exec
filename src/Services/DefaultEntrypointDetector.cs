using System.Runtime.InteropServices;

namespace Ivanize.DotnetTool.Exec
{
  public class DefaultEntrypointDetector : IDefaultEntrypointDetector
  {
    public Entrypoint GetDefaultEntrypoint()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return Entrypoint.Windows;
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        return Entrypoint.Unix;
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        return Entrypoint.Unix;
      if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        return Entrypoint.Unix;

      return Entrypoint.Unix;
    }
  }
}
