using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class SubContainerResolutionSource : ISubContainerResolution
    {
        private readonly IRegistration registration;
        private readonly TypeAssignmentSource source;

        public SubContainerResolutionSource(IRegistration registration, Type type)
        {
            this.registration = registration;
            source = new TypeAssignmentSource(type);
        }

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public IInstantiation CreateInstantiation(IObjectScope scope)
        {
            registration.Register(scope);

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

        public ITypeAssignment ResolvedImmediately()
        {
            var execution = new ExecuteToResolveImmediately(source);

            registration.Register(execution);

            return this;
        }
    }
}
