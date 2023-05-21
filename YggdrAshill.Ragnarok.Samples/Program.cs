namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Program
    {
        private static void Main(string[] arguments)
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<ISender, ConsoleSender>().WithArgument("announcement", "Please enter a text");
            context.RegisterLocal<IReceiver, ConsoleReceiver>()
                .WithFieldInjection()
                .With("header", "Received");
            context.RegisterInstance(Formatter.AllCharactersToUpper)
                .As<IFormatter>();
            context.RegisterGlobal<IService, Service>()
                .WithPropertyInjection()
                .With("Announcement", "Sample application started.\nEnter \"quit\" if you want to quit this application.\n");

            using var scope = context.Build();

            scope.Resolver.Resolve<IService>().Run();
        }
    }
}
