dotnet-exec
============

[![Build Status](https://travis-ci.com/javadparvaresh/dotnet-exec.svg?branch=master)](https://travis-ci.com/javadparvaresh/dotnet-exec)

A tool that allows you easily execute your custom commands.

Any custom command can be .NET CLI command, it just has to be in defined in root path as `.dotnetexec.json` file.

> This is a community project, free and open source. Everyone is invited to contribute, fork, share and use the code.

## Quick Start

install the tool by using to following command:
```
dotnet tool install --global dotnet-tool-exec
```

Create `.dotnetexec.json` file in root of your solution and define your comamnds:
```json
{
    "name":"MySolution",
    "env":{
        "NAME":"MY_NAME"
    },
	"entrypoint":"/bin/bash",
	"commands":{
		"test":[ 
            "dotnet build",
            "dotnet test -p $NAME",
        ]
	}
}
```

> Entrypoint is an optional parameter, automaticly will be detected based on operation system

> Commands will concat by '&&' and executed in one single line


To execute a command use `dotnet execute [command]`. ex:
```shell
dotnet execute test
```

# How to contribute

Just fork the project, make your changes send us a PR.
You can compile the project with Visual Studio 2017 and/or the .NET Core 2.0 CLI!

In the root folder, just run:

dotnet restore
dotnet build
dotnet test
