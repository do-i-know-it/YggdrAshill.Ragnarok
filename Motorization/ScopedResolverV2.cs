using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ScopedResolverV2 : IScopedResolverV2
    {
        private readonly Compilation compilation;
        private readonly IScopedResolverV2? parent;

        private readonly Dictionary<Type, IDepiction?> dictionary;

        public ScopedResolverV2(IDictionary<Type, IDepiction?> content, Compilation compilation, IScopedResolverV2? parent)
        {
            dictionary = new Dictionary<Type, IDepiction?>(content);
            this.compilation = compilation;
            this.parent = parent;
        }

        private readonly ConcurrentDictionary<Type, IDepiction> registrationCache
            = new ConcurrentDictionary<Type, IDepiction>();
        private readonly ConcurrentDictionary<IDepiction, object> instanceCache
            = new ConcurrentDictionary<IDepiction, object>();
        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

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

            IScopedResolverV2 resolver = this;

            while (true)
            {
                if (resolver.CanResolve(type, out var depiction))
                {
                    var lifetime = depiction.Lifetime;

                    switch (lifetime)
                    {
                        case Lifetime.Global:
                            return resolver.Resolve(depiction);
                        case Lifetime.Local:
                            return ResolveLocally(depiction);
                        case Lifetime.Temporal:
                            return ResolveTemporally(depiction);
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

        private object ResolveLocally(IDepiction depiction)
        {
            return instanceCache.GetOrAdd(depiction, ResolveTemporally);
        }

        private object ResolveTemporally(IDepiction depiction)
        {
            var instance = depiction.Instantiate(this);
            var ownership = depiction.Ownership;

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

        public bool CanEscalate(out IScopedResolverV2 resolver)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            resolver = parent!;

            return parent != null;
        }

        public bool CanResolve(Type type, out IDepiction depiction)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            depiction = default!;

            if (dictionary.TryGetValue(type, out var found) && found != null)
            {
                depiction = found;

                return true;
            }

            return CanResolveCollection(type, out depiction) || CanResolveServiceBundle(type, out depiction);
        }

        private bool CanResolveCollection(Type type, out IDepiction depiction)
        {
            depiction = default!;

            if (!CollectionDepiction.TryToGetElementType(type, out var elementType))
            {
                return false;
            }

            var implementedType = CollectionDepiction.GetImplementedType(elementType);

            depiction = registrationCache.GetOrAdd(implementedType, _ =>
            {
                var activation = compilation.GetActivation(implementedType);

                if (!CanResolve(elementType, out var elementDepiction))
                {
                    return new CollectionDepiction(elementType, activation, Array.Empty<IDepiction>());
                }

                return new CollectionDepiction(elementType, activation, new[] { elementDepiction! });
            });

            return true;
        }

        private bool CanResolveServiceBundle(Type type, out IDepiction depiction)
        {
            depiction = default!;

            if (!ServiceBundleDepiction.TryToGetTargetType(type, out var targetType))
            {
                return false;
            }

            if (CanResolve(targetType, out var found) && found is CollectionDepiction collection)
            {
                depiction = registrationCache.GetOrAdd(type, _ =>
                {
                    var activation = compilation.GetActivation(type);

                    return new ServiceBundleDepiction(type, activation, collection);
                });

                return true;
            }

            return false;
        }

        public object Resolve(IDepiction depiction)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            var lifetime = depiction.Lifetime;

            switch (lifetime)
            {
                case Lifetime.Global:
                {
                    if (parent == null || dictionary.ContainsKey(depiction.ImplementedType))
                    {
                        return ResolveLocally(depiction);
                    }

                    return parent.Resolve(depiction);
                }
                case Lifetime.Local:
                    return ResolveLocally(depiction);
                case Lifetime.Temporal:
                    return ResolveTemporally(depiction);
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

            return new ScopedResolverBuilder(compilation, this);
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
