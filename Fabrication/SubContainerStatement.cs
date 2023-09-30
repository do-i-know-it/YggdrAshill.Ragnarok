using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class SubContainerStatement : IStatement
    {
        private readonly IObjectContainer container;
        private readonly IInstallation[] installationList;

        public Type ImplementedType { get; }

        public SubContainerStatement(Type implementedType, IObjectContainer container, IInstallation[] installationList)
        {
            ImplementedType = implementedType;
            this.container = container;
            this.installationList = installationList;
        }

        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;

        private IInstantiation? instantiation;
        public IInstantiation Instantiation
        {
            get
            {
                if (instantiation == null)
                {
                    var scope = container.CreateScope(installationList);

                    container.Registration.Register(scope);

                    instantiation = new InstantiateFromOtherResolver(ImplementedType, scope.Resolver);
                }

                return instantiation;
            }
        }
    }
}
