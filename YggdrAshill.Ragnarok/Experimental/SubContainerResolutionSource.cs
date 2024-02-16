using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class SubContainerResolutionSource : ISubContainerResolution
    {
        private readonly TypeAssignmentSource source;

        public IObjectResolver Resolver { get; }

        public SubContainerResolutionSource(IObjectResolver resolver, Type type)
        {
            source = new TypeAssignmentSource(type);
            Resolver = resolver;
        }

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        private readonly List<IInstallation> installationList = new();

        public IReadOnlyList<IInstallation> InstallationList => installationList;

        public ISubContainerResolution With(IInstallation installation)
        {
            if (!installationList.Contains(installation))
            {
                installationList.Add(installation);
            }

            return this;
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
    }
}
