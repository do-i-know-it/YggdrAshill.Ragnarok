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

    internal sealed class InternalFactory<TInput, TOutput> : IFactory<TInput, TOutput>, IDisposable, IInstallation, ICreation<TInput>
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IObjectContext context;

        public InternalFactory(IObjectContext context)
        {
            this.context = context;
        }

        private readonly List<IDisposable> disposableList = new();

        private TInput? cache;

        public TOutput Create(TInput input)
        {
            cache = input;

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

        public void Install(IObjectContainer container)
        {
            container.RegisterInstance(this, Lifetime.Temporal);
        }

        public TInput Create()
        {
            if (cache == null)
            {
                throw new RagnarokNotRegisteredException(typeof(TInput));
            }

            return cache;
        }
    }
}
