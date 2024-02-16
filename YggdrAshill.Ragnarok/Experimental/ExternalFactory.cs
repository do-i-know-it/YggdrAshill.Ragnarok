using System.Collections.Generic;

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

    internal sealed class ExternalFactory<TInput, TOutput> : IFactory<TInput, TOutput>
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IObjectContainer container;
        private readonly IReadOnlyList<IInstallation> installationList;

        public ExternalFactory(IObjectContainer container, IReadOnlyList<IInstallation> installationList)
        {
            this.container = container;
            this.installationList = installationList;
        }

        public TOutput Create(TInput input)
        {
            var context = container.CreateContext();

            context.Install(installationList);

            context.RegisterInstance(input);

            var scope = context.CreateScope();

            return scope.Resolver.Resolve<TOutput>();
        }
    }
}
