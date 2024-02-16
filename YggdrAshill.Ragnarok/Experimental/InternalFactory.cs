using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InternalFactory<T> : IFactory<T>, IDisposable
        where T : notnull
    {
        private readonly IObjectContext context;

        public InternalFactory(IObjectContext context)
        {
            this.context = context;
        }

        private readonly List<IDisposable> disposableList = new();

        public T Create()
        {
            var scope = context.CreateScope();

            disposableList.Add(scope);

            return scope.Resolver.Resolve<T>();
        }

        public void Dispose()
        {
            foreach (var disposable in disposableList)
            {
                disposable.Dispose();
            }

            disposableList.Clear();
        }
    }

    internal sealed class InternalFactory<TInput, TOutput> : IFactory<TInput, TOutput>, IDisposable
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IObjectContainer container;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InternalFactory(IObjectContainer container, IReadOnlyList<IInstallation> installationList)
        {
            this.container = container;
            this.installationList = installationList;
        }

        private readonly List<IDisposable> disposableList = new();

        public TOutput Create(TInput input)
        {
            var context = container.CreateContext();

            context.Install(installationList);

            context.RegisterInstance(input);

            var scope = context.CreateScope();

            disposableList.Add(scope);

            return scope.Resolver.Resolve<TOutput>();
        }

        public void Dispose()
        {
            foreach (var disposable in disposableList)
            {
                disposable.Dispose();
            }

            disposableList.Clear();
        }
    }
}
