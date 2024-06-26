# YggdrAshill.Ragnarok: a lifecycle management framework

![GitHub](https://img.shields.io/github/license/do-i-know-it/YggdrAshill.Ragnarok)
![GitHub Release Date](https://img.shields.io/github/release-date/do-i-know-it/YggdrAshill.Ragnarok)
![GitHub last commit](https://img.shields.io/github/last-commit/do-i-know-it/YggdrAshill.Ragnarok)
![GitHub repo size](https://img.shields.io/github/repo-size/do-i-know-it/YggdrAshill.Ragnarok)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/do-i-know-it/YggdrAshill.Ragnarok)

Ragnarok defines how to manage lifecycles of applications for mainly XR (VR/AR/MR), inspired by

- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/)
- [Autofac](https://autofac.org/)
- [Zenject](https://github.com/modesttree/Zenject)/[Extenject](https://github.com/Mathijs-Bakker/Extenject)/[UniDi](https://github.com/UniDi/UniDi)
- [VContainer](https://vcontainer.hadashikick.jp/)

This framework isolates definitions from implementations for the lifecycle, allowing you to implement and extend it to adapt to the platforms you use, as shown below.

- ex) [Unity](https://unity.com/ja)
- ex) [Xamarin](https://docs.microsoft.com/ja-jp/xamarin/get-started/what-is-xamarin)
- ex) [ASP.NET Core](https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/host/generic-host)
- ex) [Generic Host](https://docs.microsoft.com/ja-jp/dotnet/core/extensions/generic-host)
- ex) [Windows Presentation Foundation (WPF)](https://docs.microsoft.com/ja-jp/visualstudio/designers/getting-started-with-wpf)

## Dependencies

This framework depends on .NET Standard 2.0.

## Installation

Developers should

1. Go to [Release pages](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/releases).
1. Download the latest version.

to use this framework.

## Usage

Using our framework, you can resolve dependencies as below:
```cs
interface ISender
{
    string Send();
}

interface IReceiver
{
    void Receive(string message);
}

class Service
{
    readonly ISender sender;
    readonly IReciever receiver;

    [Inject]
    Service(ISender sender, IReceiver receiver)
    {
        this.sender = sender;
        this.receiver = receiver;
    }

    public void Run()
    {
        var message = sender.Send();
        receiver.Receive(message);
    }
}
```
using implementations as below:

```cs
class ConsoleSender : ISender
{
    readonly string announcement;

    [Inject]
    ConsoleSender(string announcement)
    {
        this.announcement = announcement;
    }

    public string Send()
    {
        Console.Write($"{announcement}:");

        return Console.ReadLine() ?? string.Empty;
    }
}

class ConsoleReceiver : IReceiver
{
    [InjectField]
    readonly string? header;

    public void Receive(string message)
    {
        if (header == null)
        {
            Console.WriteLine($"{message}");
        }
        else
        {
            Console.WriteLine($"{header}: {message}");
        }
    }
}
```

like:
```cs
var context = new DependencyInjectionContext();

// Register ConsoleSender as ISender to instantiate per local scope.
context.Register<ConsoleSender>(Lifetime.Local)
    .WithArgument("announcement", "Enter any text") // Add parameter to inject into constructor.
    .As<ISender>();
// Register ConsoleReceiver as IReceiver to instantiate per global scope.
context.Register<ConsoleReceiver>(Lifetime.Global)
    .WithField("header", "Recieved") // Add parameter to inject into fields.
    .As<IReceiver>();
// Register Service to instantiate per request.
context.Register<Service>(Lifetime.Temporal);

using var scope = context.CreateScope();

// You can resolve an instance of Service and run it.
scope.Resolver.Resolve<Service>().Run();
```

or using `IInstallation`s as below:

```cs
class ServiceInstallation : IInstallation
{
    public void Install(IObjectContainer container)
    {
        container.Register<ISender, ConsoleSender>(Lifetime.Local).WithArgument("announcement", "Enter any text");
        container.Register<IReceiver, ConsoleReceiver>(Lifetime.Global).WithField("header", "Recieved");
        container.Register<Service>(Lifetime.Temporal);
    }
}
```

like:
```cs
var context = new DependencyInjectionContext();

// You can register installation like:
context.Install<ServiceInstallation>();
// or 
context.Install(new ServiceInstallation());

using var scope = context.CreateScope();

// The result is like:
// "Enter any text: This is a test."
// "Recieved: This is a test."
scope.Resolver.Resolve<Service>().Run();
```

### __Instantiating collection__

In our framework, you can resolve collection as bellow:
```cs
interface ILogger
{
    void Log(string message);
}

class ConsoleLogger : ILogger
{
    ...
}

class FileLogger : ILogger
{
    ...
}
```

like:
```cs
var context = new DependencyInjectionContext();

context.Register<ILogger, ConsoleLogger>(Lifetime.Global); // Same as Register<ConsoleLogger>(Lifetime.Global).As<ILogger>();
context.Register<ILogger, FileLogger>(Lifetime.Global);

using var scope = context.CreateScope();

// You can resolve collection like:
var array = scope.Resolver.Resolve<ILogger[]>();
// or
var readOnlyList = scope.Resolver.Resolve<IReadOnlyList<ILogger>>();
// or
var readOnlyCollection = scope.Resolver.Resolve<IReadOnlyCollection<ILogger>>();
// or
var enumerable = scope.Resolver.Resolve<IEnumerable<ILogger>>();
```

### __Resolving dependency from sub containers__

You can resolve instance using sub containers like:

```cs
var context = new DependencyInjectionContext();

context.RegisterFromSubContainer<Service>(subContainer =>
{
    subContainer.Register<ISender, ConsoleSender>(Lifetime.Global).WithArgument("announcement", "Enter any text");
    subContainer.Register<IReceiver, ConsoleReceiver>(Lifetime.Global).WithField("header", "Recieved");
    subContainer.Register<Service>(Lifetime.Temporal);
});

using var scope = context.CreateScope();

// You can resolve an instance of Service.
var service = scope.Resolver.Resolve<Service>();
// but you can't resolve an instance of ISender.
var sender = scope.Resolver.Resolve<ISender>(); // RagnarokNotRegisteredException.
```

Sub containers (inspired by [Zenject/Extenject/UniDi](https://github.com/UniDi/UniDi)) help you making dependency scopes more smaller.

### __Resolving factory to create instance on demand__

You can resolve instance using sub containers like:

```cs
var context = new DependencyInjectionContext();

context.RegisterFactory<Service, ServiceInstallation>(Ownership.External);

using var scope = context.CreateScope();

var facotry = scope.Resolver.Resolve<IFactory<Service>>();
var service = factory.Create();

service.Run();
```

Registering factories (inspired by [Zenject/Extenject/UniDi](https://github.com/UniDi/UniDi) and [VContainer](https://vcontainer.hadashikick.jp/)) help you creating instances on demand.

## Architecture

Now writing...

## Known issues

Please see [GitHub Issues](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/issues).

## Future works

Please see [GitHub Projects](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/projects/1).

- Implementations using IL emission or Source Generator.
- Decolation/Interception (inspired by [Autofac](https://autofac.org/))
- Configuration (inspired by [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/) and [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/))

## License

This framework is under the [MIT License](https://opensource.org/licenses/mit-license.php), see [LICENSE](./LICENSE.md).

## Remarks

This framework is a part of YggdrAshill framework.  
Other frameworks will be produced soon for YggdrAshill.
