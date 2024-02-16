namespace YggdrAshill.Ragnarok
{
    internal sealed class ExternalFactory<T> : IFactory<T>
        where T : notnull
    {
        private readonly IObjectContext context;

        public ExternalFactory(IObjectContext context)
        {
            this.context = context;
        }

        public T Create()
        {
            var scope = context.CreateScope();

            return scope.Resolver.Resolve<T>();
        }
    }

    internal sealed class ExternalFactory<TInput, TOutput> : IFactory<TInput, TOutput>, IInstallation, ICreation<TInput>
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IObjectContext context;

        public ExternalFactory(IObjectContext context)
        {
            this.context = context;
        }

        private TInput? cache;

        public TOutput Create(TInput input)
        {
            cache = input;

            var scope = context.CreateScope();

            return scope.Resolver.Resolve<TOutput>();
        }

        public void Install(IObjectContainer container)
        {
            container.RegisterInstance(this, Lifetime.Temporal);
        }

        public TInput Create()
        {
            if (cache == null)
            {
                throw new RagnarokNotRegisteredException(typeof(TInput));
            }

            return cache;
        }
    }
}
