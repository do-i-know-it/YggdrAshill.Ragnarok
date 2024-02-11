using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainerStatement : IStatement
    {
        private readonly IObjectContainer container;
        private readonly IInstallation installation;
        private readonly Lazy<IInstantiation> instantiation;

        public SubContainerSource Source { get; }

        public ResolveFromSubContainerStatement(Type type, IObjectContainer container, IInstallation installation)
        {
            this.container = container;
            this.installation = installation;
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Source = new SubContainerSource(container.Registration, type);
        }

        private IInstantiation CreateInstantiation()
        {
            var context = container.CreateContext();

            installation.Install(context);

            var scope = context.CreateScope();

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
