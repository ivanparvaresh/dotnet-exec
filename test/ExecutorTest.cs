using System;
using Xunit;

namespace Ivanize.DotnetTool.Exec.Test
{
  public class ExecutorTest
  {
    [Fact]
    public void Constructor_Should_Use_Console_As_Ouput_Writers()
    {
      var executor = new Executor();


      Assert.Equal(executor.ErrorWriter, Console.Error);
      Assert.Equal(executor.OutWriter, Console.Out);
    }
    [Fact]
    public void Execute_Should_Throw_If_Package_Is_Null()
    {
      Assert.Throws<ArgumentNullException>(() => new Executor().Execute(null, new string[] { }));
    }
    [Fact]
    public void Execute_Should_Throw_If_Args_Are_Empty()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] { }
      );
      var executor = new Executor();
      Assert.Throws<ArgumentNullException>(() => executor.Execute(pkg, null));
    }
    [Fact]
    public void Execute_Should_Print_Help_Message_If_Argument_Is_Empty()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] { }
      );

      var stringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(stringBuilder);
      var executor = new Executor(outWriter, outWriter);


      executor.Execute(pkg, new string[] { });
      outWriter.Flush();

      Assert.Equal(
          "\n\nUsage:  [options]\n\nOptions:\n  -h  Show help information\n\n",
          stringBuilder.ToString());

      outWriter.Close();
    }

    [Fact]
    public void Execute_Should_Print_Help_Message_Comamnds_Included_If_Argument_Is_Empty()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] {
                    new Command("start",new string[]{"dotnet start"})
          }
      );

      var stringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(stringBuilder);
      var executor = new Executor(outWriter, outWriter);


      executor.Execute(pkg, new string[] { });
      outWriter.Flush();

      Assert.Equal(
          "\n\nUsage:  [options] [command]\n\nOptions:\n  -h  Show help information\n\nCommands:\n  start   Run start command\n\nUse \" [command] --\" for more information about a command.\n\n",
          stringBuilder.ToString());
      outWriter.Close();
    }

    [Fact]
    public void Execute_Should_Print_Help_Message_Comamnds_Included_If_Argument_pass_help_param()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] {
                    new Command("start",new string[]{"dotnet start"})
          }
      );

      var stringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(stringBuilder);
      var executor = new Executor(outWriter, outWriter);


      executor.Execute(pkg, new string[] { "-h" });
      outWriter.Flush();

      Assert.Equal(
          "\n\nUsage:  [options] [command]\n\nOptions:\n  -h  Show help information\n\nCommands:\n  start   Run start command\n\nUse \" [command] --\" for more information about a command.\n\n",
          stringBuilder.ToString());
      outWriter.Close();
    }

    [Fact]
    public void Execute_Should_Execute_The_Singleline_Command_Script()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] {
                    new Command("start",new string[]{"echo 'Start'"})
          }
      );

      var outputStringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(outputStringBuilder);

      var errorStringBuilder = new System.Text.StringBuilder();
      var errorWriter = new System.IO.StringWriter(errorStringBuilder);

      var executor = new Executor(outWriter, errorWriter);


      executor.Execute(pkg, new string[] { "start" });
      outWriter.Flush();

      Assert.Equal("Start\n\n", outputStringBuilder.ToString());
      outWriter.Close();
    }

    [Fact]
    public void Execute_Should_Execute_The_Multiline_Command_Script()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] {
                    new Command("start",new string[]{
                        "echo 'Start'",
                        "echo 'Finish'",
                    })
          }
      );

      var outputStringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(outputStringBuilder);

      var errorStringBuilder = new System.Text.StringBuilder();
      var errorWriter = new System.IO.StringWriter(errorStringBuilder);

      var executor = new Executor(outWriter, errorWriter);


      executor.Execute(pkg, new string[] { "start" });
      outWriter.Flush();

      Assert.Equal("Start\nFinish\n\n", outputStringBuilder.ToString());
      outWriter.Close();
    }

    [Fact]
    public void Execute_Should_Capture_The_StandardOutput()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] {
                    new Command("start",new string[]{"echo 'Start'"})
          }
      );

      var outputStringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(outputStringBuilder);

      var errorStringBuilder = new System.Text.StringBuilder();
      var errorWriter = new System.IO.StringWriter(errorStringBuilder);

      var executor = new Executor(outWriter, errorWriter);


      executor.Execute(pkg, new string[] { "start" });
      outWriter.Flush();

      Assert.Equal("Start\n\n", outputStringBuilder.ToString());
      outWriter.Close();
    }

    [Fact]
    public void Execute_Should_Capture_The_ErrorOutput()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] { },
          commands: new Command[] {
                    new Command("start",new string[]{"not-existed-command do-something"})
          }
      );

      var outputStringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(outputStringBuilder);

      var errorStringBuilder = new System.Text.StringBuilder();
      var errorWriter = new System.IO.StringWriter(errorStringBuilder);

      var executor = new Executor(outWriter, errorWriter);


      executor.Execute(pkg, new string[] { "start" });
      errorWriter.Flush();

      Assert.Equal("/bin/bash: not-existed-command: command not found\n\n", errorStringBuilder.ToString());
      outWriter.Close();
    }

    [Fact]
    public void Execute_Should_Pass_Environment_Variable_To_The_Command_Script()
    {
      var pkg = new Package(
          entrypoint: Entrypoint.Unix,
          variables: new EnvVariable[] {
                    new EnvVariable("PROJ_NAME","TEST")
          },
          commands: new Command[] {
                    new Command("start",new string[]{"echo \"$PROJ_NAME\""})
          }
      );

      var outputStringBuilder = new System.Text.StringBuilder();
      var outWriter = new System.IO.StringWriter(outputStringBuilder);

      var errorStringBuilder = new System.Text.StringBuilder();
      var errorWriter = new System.IO.StringWriter(errorStringBuilder);

      var executor = new Executor(outWriter, errorWriter);


      executor.Execute(pkg, new string[] { "start" });
      outWriter.Flush();

      Assert.Equal("TEST\n\n", outputStringBuilder.ToString());
      outWriter.Close();
    }
  }
}
