using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainerStatement : IStatement, ISubContainerResolution
    {
        private readonly IObjectContainer container;
        private readonly Lazy<IInstantiation> instantiation;

        private readonly TypeAssignmentSource source;

        public ResolveFromSubContainerStatement(Type type, IObjectContainer container)
        {
            this.container = container;
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            source = new TypeAssignmentSource(type);
        }

        private readonly List<IInstallation> installationList = new();

        public IObjectResolver Resolver => container.Resolver;

        public ISubContainerResolution With(IInstallation installation)
        {
            if (!installationList.Contains(installation))
            {
                installationList.Add(installation);
            }

            return this;
        }

        private IInstantiation CreateInstantiation()
        {
            var context = container.CreateContext();

            InstallationList.Install(context, installationList);

            var scope = context.CreateScope();

            container.Registration.Register(scope);

            return new InstantiateFromSubContainer(ImplementedType, scope.Resolver);
        }

        public void AsOwnSelf()
        {
            source.AsOwnSelf();
        }

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            return source.As(inheritedType);
        }

        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            return source.AsImplementedInterfaces();
        }

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Temporal;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
