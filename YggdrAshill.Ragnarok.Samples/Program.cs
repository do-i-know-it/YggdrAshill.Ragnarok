namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Program
    {
        private static void Main(string[] arguments)
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<ConsoleSender>()
                .WithArgument("announcement", "Please enter a text")
                .As<ISender>();
            context.RegisterLocal<ConsoleReceiver>()
                .WithFieldInjection()
                .With("header", "Received")
                .As<IReceiver>();
            context.RegisterInstance(Formatter.AllCharactersToUpper)
                .As<IFormatter>();
            context.RegisterGlobal<Service>();

            using var scope = context.Build();

            scope.Resolver.Resolve<Service>().Run();
        }
    }
}
