using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnFactoryStatement<T> : IStatement
        where T : notnull
    {
        private readonly IObjectContainer container;
        private readonly Lazy<IInstantiation> instantiation;

        public Ownership Ownership { get; }
        public FactoryResolutionSource Source { get; }

        public ReturnFactoryStatement(IObjectContainer container, Ownership ownership)
        {
            this.container = container;
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Ownership = ownership;
            Source = new FactoryResolutionSource(container.Resolver);
        }

        private IInstantiation CreateInstantiation()
        {
            var factory = CreateFactory();

            return new InstantiateToReturnInstance(factory);
        }

        private IFactory<T> CreateFactory()
        {
            var context = container.CreateContext();

            context.Install(Source.InstallationList);

            return Ownership switch
            {
                Ownership.Internal => new InternalFactory<T>(context),
                Ownership.External => new ExternalFactory<T>(context),
                _ => throw new InvalidOperationException($"{Ownership} is invalid."),
            };
        }

        public Type ImplementedType { get; } = typeof(IFactory<T>);

        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();

        public Lifetime Lifetime => Lifetime.Global;

        public IInstantiation Instantiation => instantiation.Value;
    }

    internal sealed class ReturnFactoryStatement<TInput, TOutput> : IStatement
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IObjectContainer container;
        private readonly Lazy<IInstantiation> instantiation;
        public Ownership Ownership { get; }
        public FactoryResolutionSource Source { get; }

        public ReturnFactoryStatement(IObjectContainer container, Ownership ownership)
        {
            this.container = container;
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Ownership = ownership;
            Source = new FactoryResolutionSource(container.Resolver);
        }

        private IInstantiation CreateInstantiation()
        {
            var factory = CreateFactory();

            return new InstantiateToReturnInstance(factory);
        }

        private IFactory<TInput, TOutput> CreateFactory()
        {
            return Ownership switch
            {
                Ownership.Internal => new InternalFactory<TInput, TOutput>(container, Source.InstallationList),
                Ownership.External => new ExternalFactory<TInput, TOutput>(container, Source.InstallationList),
                _ => throw new InvalidOperationException($"{Ownership} is invalid."),
            };
        }

        public Type ImplementedType { get; } = typeof(IFactory<TInput, TOutput>);

        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();

        public Lifetime Lifetime => Lifetime.Global;

        public IInstantiation Instantiation => instantiation.Value;
    }
}
