namespace YggdrAshill.Ragnarok.Samples
{
    internal sealed class Program
    {
        private static void Main(string[] arguments)
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<MessageSender>()
                .WithArgument("message", "Hello world")
                .As<ISender>();
            context.RegisterInstance(ConsoleReceiver.Instance).As<IReceiver>();
            context.RegisterGlobal<Service>();

            using (var scope = context.Build())
            {
                var service = scope.Resolver.Resolve<Service>();

                service.Run();
            }
        }
    }
}
