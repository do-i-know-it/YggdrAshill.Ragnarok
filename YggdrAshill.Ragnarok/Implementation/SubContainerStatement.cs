using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class SubContainerStatement : ITypeAssignment, IStatement
    {
        private readonly IObjectContainer container;
        private readonly IInstallation[] installationList;
        private readonly TypeAssignmentStatement typeAssignment;

        public SubContainerStatement(Type type, IObjectContainer container, IInstallation[] installationList)
        {
            this.container = container;
            this.installationList = installationList;
            typeAssignment = new TypeAssignmentStatement(type, Lifetime.Temporal, Ownership.External, CreateInstantiation);
        }

        private IInstantiation? instantiation;
        private IInstantiation CreateInstantiation()
        {
            if (instantiation == null)
            {
                var scope = container.CreateSubScope(installationList);

                container.Registration.Register(scope);

                instantiation = new InstantiateFromOtherResolver(ImplementedType, scope.Resolver);
            }

            return instantiation;
        }

        public Type ImplementedType => typeAssignment.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => typeAssignment.AssignedTypeList;
        public Lifetime Lifetime => typeAssignment.Lifetime;
        public Ownership Ownership => typeAssignment.Ownership;
        public IInstantiation Instantiation => typeAssignment.Instantiation;

        public void AsOwnSelf()
        {
            typeAssignment.AsOwnSelf();
        }
        public IInheritedTypeAssignment As(Type inheritedType)
        {
            return typeAssignment.As(inheritedType);
        }
        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            return typeAssignment.AsImplementedInterfaces();
        }
    }
}
