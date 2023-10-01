namespace YggdrAshill.Ragnarok.Samples
{
    internal static class Program
    {
        private static void Main(string[] arguments)
        {
            var context = new DependencyContext();

            context.Register<ISender, ConsoleSender>(Lifetime.Global)
                .WithArgument("announcement", "Please enter a text");
            context.Register<IReceiver, ConsoleReceiver>(Lifetime.Global)
                .WithField("header", "Received");
            context.RegisterInstance(Formatter.AllCharactersToUpper)
                .As<IFormatter>();
            context.Register<IService, Service>(Lifetime.Global)
                .WithProperty("Announcement", "Sample application started.\nEnter \"quit\" if you want to quit this application.\n");

            using var scope = context.CreateScope();

            scope.Resolver.Resolve<IService>().Run();
        }
    }
}
