using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateExternalFactory<T> : IInstantiation
        where T : notnull
    {
        private readonly IObjectContainer container;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateExternalFactory(IObjectContainer container, IReadOnlyList<IInstallation> installationList)
        {
            this.container = container;
            this.installationList = installationList;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            var context = container.CreateContext();

            context.Install(installationList);

            return new ExternalFactory<T>(context);
        }
    }

    internal sealed class InstantiateExternalFactory<TInput, TOutput> : IInstantiation
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IObjectContainer container;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateExternalFactory(IObjectContainer container, IReadOnlyList<IInstallation> installationList)
        {
            this.container = container;
            this.installationList = installationList;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            var context = container.CreateContext();

            context.Install(installationList);

            var factory = new ExternalFactory<TInput, TOutput>(context);

            context.Install(factory);

            return factory;
        }
    }
}
