using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ScopedResolver : IScopedResolver
    {
        private readonly Interpretation interpretation;
        private readonly IScopedResolver? parent;

        private readonly Dictionary<Type, IDescription?> dictionary;

        public ScopedResolver(IDictionary<Type, IDescription?> content, Interpretation interpretation, IScopedResolver? parent)
        {
            dictionary = new Dictionary<Type, IDescription?>(content);
            this.interpretation = interpretation;
            this.parent = parent;
        }

        private readonly ConcurrentDictionary<Type, IDescription> descriptionCache = new();
        private readonly ConcurrentDictionary<IDescription, object> instanceCache = new();
        private readonly CompositeDisposable compositeDisposable = new();

        private ScopedResolverContext? context;

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
            while (true)
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

                if (!resolver.CanEscalate(out resolver))
                {
                    break;
                }
            }

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

            return CanResolveCollection(type, out description) ||
                   CanResolveServiceBundle(type, out description) ||
                   CanResolveInstallation(type, out description);
        }

        private bool CanResolveCollection(Type type, out IDescription description)
        {
            if (!interpretation.CanResolveAsArray(type, out var elementType))
            {
                description = default!;
                return false;
            }

            description = descriptionCache.GetOrAdd(type, _ =>
            {
                if (CanResolve(elementType, out var elementDescription))
                {
                    return interpretation.CreateCollectionDescription(elementType, elementDescription);
                }
                return interpretation.CreateCollectionDescription(elementType);
            });
            return true;
        }

        private bool CanResolveServiceBundle(Type type, out IDescription description)
        {
            if (!interpretation.IsServiceBundle(type, out var elementType))
            {
                description = default!;
                return false;
            }
            var targetType = interpretation.GetReadOnlyList(elementType);
            if (!CanResolve(targetType, out var found) || found is not CollectionDescription collection)
            {
                description = default!;
                return false;
            }

            description = descriptionCache.GetOrAdd(type, _ => interpretation.CreateServiceBundleDescription(type, collection));
            return true;
        }

        private bool CanResolveInstallation(Type type, out IDescription description)
        {
            if (!InstallationDescription.CanResolve(type))
            {
                description = default!;
                return false;
            }

            description = descriptionCache.GetOrAdd(type, _ =>
            {
                var request = interpretation.GetInstantiationRequest(type);
                return new InstallationDescription(type, request);
            });
            return true;
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

        public IScopedResolverContext CreateContext()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            return context ??= new ScopedResolverContext(interpretation, this);
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
