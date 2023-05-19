# YggdrAshill.Ragnarok: a lifecycle framework

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
    private readonly ISender sender;
    private readonly IReciever receiver;

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
class AnyMessageSender : ISender
{
    private readonly string message;

    [Inject]
    AnyMessageSender(string message)
    {
        this.message = message;
    }

    public string Send()
    {
        return message;
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

// Register AnyMessageSender as ISender to instantiate per scope.
context.RegisterLocal<AnyMessageSender>()
    .WithArgument("message", "Hello world.") // Add parameter to inject into constructor.
    .As<ISender>();
// Register ConsoleReceiver as IReceiver to instantiate per scope.
context.RegisterLocal<ConsoleReceiver>()
    .WithFieldInjection() // Enable field injection.
    .With("header", "Recieved") // Add parameter to inject into fields.
    .As<IReceiver>();
// Register Service to instantiate per request.
context.RegisterTemporal<Service>();

using (var scope = context.Build())
{
    var service = scope.Resolver.Resolve<Service>();

    service.Run(); // "Recieved: Hellow world."
}
```

### Collection registrataion

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

context.RegisterGlobal<ConsoleLogger>().As<ILogger>();
context.RegisterGlobal<FileLogger>().As<ILogger>();

using (var scope = context.Build())
{
    var array = scope.Resolver.Resolve<ILogger[]>();
    var readOnlyList = scope.Resolver.Resolve<IReadOnlyList<ILogger>>();
    var readOnlyCollection = scope.Resolver.Resolve<IReadOnlyCollection<ILogger>>();
    var enumerable = scope.Resolver.Resolve<IEnumerable<ILogger>>();
}

```

## Architecture

Now writing...

## Known issues

Please see [GitHub Issues](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/issues).

## Future works

Please see [GitHub Projects](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/projects/1).

- Implementations using Expression or IL emission.
- Sub containers (inspired by [Zenject/Extenject/UniDi](https://github.com/UniDi/UniDi))
- Decolation/Interception (inspired by [Autofac](https://autofac.org/))

## License

This framework is under the [MIT License](https://opensource.org/licenses/mit-license.php), see [LICENSE](./LICENSE.md).

## Remarks

This framework is a part of YggdrAshill framework.  
Other frameworks will be produced soon for YggdrAshill.
