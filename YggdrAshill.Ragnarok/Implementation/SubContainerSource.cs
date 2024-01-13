using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class SubContainerSource : ITypeAssignment
    {
        private readonly IRegistration registration;
        private readonly TypeAssignmentSource source;

        public SubContainerSource(IRegistration registration, Type type)
        {
            this.registration = registration;
            source = new TypeAssignmentSource(type);
        }

        public IInstantiation CreateInstantiation(IObjectScope scope)
        {
            registration.Register(scope);

            return new ResolveFromSubContainer(ImplementedType, scope.Resolver);
        }

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

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
    }
}
