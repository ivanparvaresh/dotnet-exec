dotnet-exec
============

A tool that allows you easily execute your custom commands.

Any custom command can be .NET CLI command, it just has to be in defined in root path as `.dotnetexec` file.

> This is a community project, free and open source. Everyone is invited to contribute, fork, share and use the code.

## Quick Start

install the tool by using to following command:
```
dotnet tool install --global dotnet-exec
```

Create `.dotnetexec` file in root of your solution and define your comamnds:
```json
{
	"name":"MySolution",
	"entrypoint":"/bin/bash",
	"commands":{
		"test":"dotnet test -p=test.csproj"
	}
}
```

To execute a command use `dotnet execute [command]`. ex:
```shell
dotnet execute test
```
