using System.Runtime.InteropServices;

namespace Ivanize.DotnetTool.Exec
{
    public class DefaultEntrypointDetector : IDefaultEntrypointDetector
    {
        public string GetDefaultEntryPoint()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "cmd.exe";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "/bin/bash";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "/bin/bash";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return "/bin/bash";

            return "cmd.exe";
        }
    }
}
