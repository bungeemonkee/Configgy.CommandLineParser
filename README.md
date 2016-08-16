# Configgy.CommandLineParser

![Configgy: The Last Configuration Library for .NET](https://raw.githubusercontent.com/bungeemonkee/Configgy/master/icon.png)

A [Configgy](https://github.com/bungeemonkee/Configgy) extension that allows command line parsing via [CommandLineParser](https://github.com/gsscoder/commandline)

[![Build Status](https://ci.appveyor.com/api/projects/status/2r7b35779sw84c0w?svg=true)](https://ci.appveyor.com/project/bungeemonkee/configgy-commandlineparser) [![Coverage Status](https://coveralls.io/repos/github/bungeemonkee/Configgy.CommandLineParser/badge.svg?branch=master)](https://coveralls.io/github/bungeemonkee/Configgy.CommandLineParser?branch=master) [![NuGet Version](https://img.shields.io/nuget/v/Configgy.CommandLineParser.svg?maxAge=3600)](https://www.nuget.org/packages/Configgy.CommandLineParser)

## Usage

First setup [Configgy](https://github.com/bungeemonkee/Configgy) like in the [documentation](https://github.com/bungeemonkee/Configgy#usage).

Create a command line options class like so: (See [here](https://github.com/gsscoder/commandline#notes))

```csharp
class Options {
    [Option('t', "text", HelpText="Input text")]
    public string Text { get; set; }
}
```

Now add the new source to your `Config` subclass by overwriting the default constructor:

```csharp
public class MyConfig: Config, IMyConfig
{   
    public int MaxThingCount { get { return Get<int>(); } }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
    
    public MyConfig(string[] arguments)
    : base(
        new DictionaryCache(),
        new AggregateSource(
            new CommandLineParserSource<Options>(arguments)
            new EnvironmentVariableSource(),
            new FileSource(),
            new ConnectionStringsSource(),
            new AppSettingSource(),
            new EmbeddedResourceSource(),
            new DefaultValueAttributeSource()
        ),
        new AggregateTransformer(),
        new AggregateValidator(),
        new AggregateCoercer()
    )
    {
    }
}
```

This simply reproduces the normal default constructor but adds a `CommandLineParserSource` as one of the sources using the `Options` type we've already defined and gives it the arguments to parse.

## Installation

You can build from this source or you can get it from nuget here: https://www.nuget.org/packages/Configgy.CommandLineParser