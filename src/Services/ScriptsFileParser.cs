using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ivanize.DotnetTool.Exec
{
  public class ScriptsFileParser : IScriptsFileParser
  {
    private IDefaultEntrypointDetector defaultEntrypointDetector;

    public ScriptsFileParser(IDefaultEntrypointDetector defaultEntrypointDetector)
    {
      this.defaultEntrypointDetector = defaultEntrypointDetector ?? throw new ArgumentNullException(nameof(defaultEntrypointDetector));
    }

    public Package Parse(StreamReader scriptsFileStream)
    {
      if (scriptsFileStream == null)
        throw new ArgumentNullException(nameof(scriptsFileStream));

      var content = scriptsFileStream.ReadToEnd();

      InternalPackage pkgInstance = null;
      try
      {
        pkgInstance =
            JsonConvert.DeserializeObject<InternalPackage>(content);
      }
      catch (Exception ex)
      {
        throw new InvalidDataException("Unable to parse the scripts file!", ex);
      }

      if (pkgInstance == null) throw new InvalidDataException("Unable to parse the scripts file!");


      if (pkgInstance.Env.Any(s => string.IsNullOrWhiteSpace(s.Key))) throw new InvalidDataException("The Variable `name` is required!");
      if (pkgInstance.Commands.Any(s => string.IsNullOrWhiteSpace(s.Key))) throw new InvalidDataException("The Command `name` is required!");

      var entryPoint = pkgInstance.EntrypointObject ?? this.defaultEntrypointDetector.GetDefaultEntrypoint();
      var pkg = new Package(
          pkgInstance.EntrypointObject ?? this.defaultEntrypointDetector.GetDefaultEntrypoint(),
          pkgInstance.Env.Select(s => new EnvVariable(s.Key, s.Value)).ToArray(),
          pkgInstance.Commands.Select(s => new Command(s.Key, s.Value)).ToArray());

      return pkg;
    }
    // Internal classes
    private class InternalPackage
    {
      public string Entrypoint { get; set; }
      public Dictionary<string, string> Env { get; set; }
      public Dictionary<string, string[]> Commands { get; set; }

      public Entrypoint EntrypointObject
      {
        get
        {
          if (string.IsNullOrWhiteSpace(Entrypoint))
            return null;

          if (Exec.Entrypoint.Windows.Executable.Equals(Entrypoint, StringComparison.OrdinalIgnoreCase))
            return Exec.Entrypoint.Windows;

          if (Exec.Entrypoint.Unix.Executable.Equals(Entrypoint, StringComparison.OrdinalIgnoreCase))
            return Exec.Entrypoint.Unix;

          return new Entrypoint(Entrypoint, null);
        }
      }

      public InternalPackage()
      {
        this.Env = new Dictionary<string, string>();
        this.Commands = new Dictionary<string, string[]>();
      }
    }
  }
}
