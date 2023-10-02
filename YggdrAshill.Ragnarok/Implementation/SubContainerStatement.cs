using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class SubContainerStatement : ITypeAssignment, IStatement
    {
        private readonly IObjectContainer container;
        private readonly IInstallation[] installationList;
        private readonly TypeAssignmentStatement assignment;

        public SubContainerStatement(Type type, IObjectContainer container, IInstallation[] installationList)
        {
            this.container = container;
            this.installationList = installationList;
            assignment = new TypeAssignmentStatement(type, Lifetime.Temporal, Ownership.External, CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var scope = container.CreateSubScope(installationList);

            container.Registration.Register(scope);

            return new InstantiateFromOtherResolver(ImplementedType, scope.Resolver);
        }

        public Type ImplementedType => assignment.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => assignment.AssignedTypeList;

        public Lifetime Lifetime => assignment.Lifetime;

        public Ownership Ownership => assignment.Ownership;

        public IInstantiation Instantiation => assignment.Instantiation;

        public void AsOwnSelf()
        {
            assignment.AsOwnSelf();
        }

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            return assignment.As(inheritedType);
        }

        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            return assignment.AsImplementedInterfaces();
        }
    }
}
