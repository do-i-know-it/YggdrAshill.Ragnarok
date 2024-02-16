using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainerStatement : IStatement
    {
        private readonly IObjectContainer container;
        private readonly Lazy<IInstantiation> instantiation;

        public SubContainerResolutionSource Source { get; }

        public ResolveFromSubContainerStatement(Type type, IObjectContainer container)
        {
            this.container = container;
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Source = new SubContainerResolutionSource(container.Resolver, type);
        }

        private IInstantiation CreateInstantiation()
        {
            var context = container.CreateContext();

            context.Install(Source.InstallationList);

            var scope = context.CreateScope();

            container.Registration.Register(scope);

            return new InstantiateFromSubContainer(ImplementedType, scope.Resolver);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Temporal;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
