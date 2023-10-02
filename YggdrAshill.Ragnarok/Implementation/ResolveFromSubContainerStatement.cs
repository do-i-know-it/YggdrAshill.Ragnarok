using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class ResolveFromSubContainerStatement : IStatement
    {
        private readonly IObjectContainer container;
        private readonly IInstallation[] installationList;
        private readonly TypeAssignmentSource source;
        private readonly Lazy<IInstantiation> instantiation;

        public ResolveFromSubContainerStatement(Type type, IObjectContainer container, IInstallation[] installationList)
        {
            this.container = container;
            this.installationList = installationList;
            source = new TypeAssignmentSource(type);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var scope = container.CreateSubScope(installationList);

            container.Registration.Register(scope);

            return new ResolveFromSubContainer(ImplementedType, scope.Resolver);
        }

        public ITypeAssignment TypeAssignment => source;

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Temporal;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
