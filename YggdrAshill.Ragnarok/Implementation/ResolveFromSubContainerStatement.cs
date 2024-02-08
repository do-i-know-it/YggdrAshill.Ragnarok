using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainerStatement : IStatement
    {
        private readonly IObjectContainer container;
        private readonly IInstallation[] installationList;
        private readonly Lazy<IInstantiation> instantiation;

        public SubContainerSource Source { get; }

        public ResolveFromSubContainerStatement(Type type, IObjectContainer container, IInstallation[] installationList)
        {
            this.container = container;
            this.installationList = installationList;
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Source = new SubContainerSource(container.Registration, type);
        }

        private IInstantiation CreateInstantiation()
        {
            var scope = container.CreateSubScope(installationList);

            container.Registration.Register(scope);

            return Source.CreateInstantiation(scope);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Temporal;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
