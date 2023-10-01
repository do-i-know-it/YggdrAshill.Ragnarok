using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ScopedResolver : IScopedResolver
    {
        private readonly Engine engine;
        private readonly IScopedResolver? parent;

        private readonly Dictionary<Type, IDescription?> dictionary;

        public ScopedResolver(IDictionary<Type, IDescription?> content, Engine engine, IScopedResolver? parent)
        {
            dictionary = new Dictionary<Type, IDescription?>(content);
            this.engine = engine;
            this.parent = parent;
        }

        private readonly ConcurrentDictionary<Type, IDescription> registrationCache = new();
        private readonly ConcurrentDictionary<IDescription, object> instanceCache = new();
        private readonly CompositeDisposable compositeDisposable = new();

        private bool isDisposed;

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            IScopedResolver resolver = this;

            while (true)
            {
                if (resolver.CanResolve(type, out var description))
                {
                    var lifetime = description.Lifetime;

                    switch (lifetime)
                    {
                        case Lifetime.Global:
                            return resolver.Resolve(description);
                        case Lifetime.Local:
                            return ResolveLocally(description);
                        case Lifetime.Temporal:
                            return ResolveTemporally(description);
                        default:
                            throw new NotSupportedException($"{lifetime} is invalid.");
                    }
                }

                if (!resolver.CanEscalate(out resolver))
                {
                    throw new RagnarokNotRegisteredException(type);
                }
            }
        }

        private object ResolveLocally(IDescription description)
        {
            return instanceCache.GetOrAdd(description, ResolveTemporally);
        }

        private object ResolveTemporally(IDescription description)
        {
            var instance = description.Instantiate(this);
            var ownership = description.Ownership;

            switch (ownership)
            {
                case Ownership.Internal:
                    if (instance is IDisposable disposable)
                    {
                        Bind(disposable);
                    }
                    break;
                case Ownership.External:
                    break;
                default:
                    throw new NotSupportedException($"{ownership} is invalid.");
            }

            return instance;
        }

        public bool CanEscalate(out IScopedResolver resolver)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            resolver = parent!;

            return parent != null;
        }

        public bool CanResolve(Type type, out IDescription description)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            description = default!;

            if (dictionary.TryGetValue(type, out var found) && found != null)
            {
                description = found;

                return true;
            }

            return CanResolveCollection(type, out description) || CanResolveServiceBundle(type, out description);
        }

        private bool CanResolveCollection(Type type, out IDescription description)
        {
            description = default!;

            if (!CollectionDescription.CanResolve(type, out var elementType))
            {
                return false;
            }

            description = registrationCache.GetOrAdd(type, _ =>
            {
                var activation = engine.CreateActivation(type);

                if (!CanResolve(elementType, out var elementDescription))
                {
                    return new CollectionDescription(elementType, activation, Array.Empty<IDescription>());
                }

                return new CollectionDescription(elementType, activation, new[] { elementDescription! });
            });

            return true;
        }

        private bool CanResolveServiceBundle(Type type, out IDescription description)
        {
            description = default!;

            if (!ServiceBundleDescription.CanResolve(type, out var elementType))
            {
                return false;
            }

            var targetType = TypeCache.ReadOnlyListOf(elementType);

            if (CanResolve(targetType, out var found) && found is CollectionDescription collection)
            {
                description = registrationCache.GetOrAdd(type, _ =>
                {
                    var activation = engine.CreateActivation(type);

                    return new ServiceBundleDescription(type, activation, collection);
                });

                return true;
            }

            return false;
        }

        public object Resolve(IDescription description)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            var lifetime = description.Lifetime;

            switch (lifetime)
            {
                case Lifetime.Global:
                {
                    if (parent == null || dictionary.ContainsKey(description.ImplementedType))
                    {
                        return ResolveLocally(description);
                    }

                    return parent.Resolve(description);
                }
                case Lifetime.Local:
                    return ResolveLocally(description);
                case Lifetime.Temporal:
                    return ResolveTemporally(description);
                default:
                    throw new NotSupportedException($"{lifetime} is invalid.");
            }
        }

        public void Bind(IDisposable disposable)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            compositeDisposable.Add(disposable);
        }

        public IScopedResolverBuilder CreateBuilder()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            return new ScopedResolverBuilder(engine, this);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            compositeDisposable.Dispose();

            registrationCache.Clear();

            instanceCache.Clear();

            dictionary.Clear();

            isDisposed = false;
        }
    }
}
