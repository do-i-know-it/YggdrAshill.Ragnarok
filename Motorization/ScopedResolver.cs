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

        private readonly ConcurrentDictionary<Type, IDescription> descriptionCache = new();
        private readonly ConcurrentDictionary<IDescription, object> instanceCache = new();
        private readonly CompositeDisposable compositeDisposable = new();

        private bool isDisposed;

        public object Resolve(Type type)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            return Resolve(type, this);
        }

        private object Resolve(Type type, IScopedResolver resolver)
        {
            do
            {
                if (resolver.CanResolve(type, out var description))
                {
                    return description.Lifetime switch
                    {
                        Lifetime.Global => resolver.Resolve(description),
                        Lifetime.Local => ResolveLocally(description),
                        Lifetime.Temporal => ResolveTemporally(description),
                        _ => throw new NotSupportedException($"{description.Lifetime} is invalid.")
                    };
                }
            } while (resolver.CanEscalate(out resolver));

            throw new RagnarokNotRegisteredException(type);
        }

        private object ResolveLocally(IDescription description)
        {
            return instanceCache.GetOrAdd(description, ResolveTemporally);
        }

        private object ResolveTemporally(IDescription description)
        {
            switch (description.Ownership)
            {
                case Ownership.Internal:
                    var instance = description.Instantiate(this);

                    if (instance is IDisposable disposable)
                    {
                        Bind(disposable);
                    }

                    return instance;
                case Ownership.External:
                    return description.Instantiate(this);
                default:
                    throw new NotSupportedException($"{description.Ownership} is invalid.");
            }
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

            if (dictionary.TryGetValue(type, out var found))
            {
                description = found!;

                return found != null;
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

            description = descriptionCache.GetOrAdd(type, _ =>
            {
                var activation = engine.GetActivation(type);

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
                description = descriptionCache.GetOrAdd(type, _ =>
                {
                    var activation = engine.GetActivation(type);

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

            switch (description.Lifetime)
            {
                case Lifetime.Global:
                    if (parent == null || dictionary.ContainsKey(description.ImplementedType))
                    {
                        return ResolveLocally(description);
                    }
                    return parent.Resolve(description);
                case Lifetime.Local:
                    return ResolveLocally(description);
                case Lifetime.Temporal:
                    return ResolveTemporally(description);
                default:
                    throw new NotSupportedException($"{description.Lifetime} is invalid.");
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

            descriptionCache.Clear();

            instanceCache.Clear();

            dictionary.Clear();

            isDisposed = false;
        }
    }
}
