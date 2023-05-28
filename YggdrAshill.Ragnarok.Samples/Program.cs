namespace YggdrAshill.Ragnarok.Samples
{
    internal static class Program
    {
        private static void Main(string[] arguments)
        {
            var context = new DependencyContext();

            context.Register<ConsoleSender>(Lifetime.Global);

            context.Register<ISender, ConsoleSender>(Lifetime.Global)
                .WithArgument("announcement", "Please enter a text");
            context.Register<IReceiver, ConsoleReceiver>(Lifetime.Global)
                .WithFieldsInjected()
                .From("header", "Received");
            context.RegisterInstance(Formatter.AllCharactersToUpper)
                .As<IFormatter>();
            context.Register<IService, Service>(Lifetime.Global)
                .WithPropertiesInjected()
                .From("Announcement", "Sample application started.\nEnter \"quit\" if you want to quit this application.\n");

            using var scope = context.Build();

            scope.Resolver.Resolve<IService>().Run();
        }
    }
}
