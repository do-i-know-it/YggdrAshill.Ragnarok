using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok.Motorization
{
    internal sealed class ScopedResolver :
        IScopedResolver
    {
        private readonly IScopedResolver? parent;
        private readonly IEngine engine;
        private readonly IEngineBuilder engineBuilder;

        public ScopedResolver(IScopedResolver? parent, IEngine engine, IEngineBuilder engineBuilder)
        {
            this.parent = parent;
            this.engine = engine;
            this.engineBuilder = engineBuilder;
        }

        private bool isDisposed;

        public object Resolve(Type type)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            if (engine.Find(type, out var registration))
            {
                return ResolveInternally(registration);
            }

            if (parent != null)
            {
                return parent.Resolve(type, this);
            }

            throw new RagnarokNotRegisteredException(type);
        }

        private object ResolveInternally(IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IEngine));
            }

            var lifetime = registration.Lifetime;

            switch (lifetime)
            {
                case Lifetime.Global:
                case Lifetime.Local:
                    return engine.GetInstance(registration, Instantiate);
                case Lifetime.Temporal:
                    return Instantiate(registration);
                default:
                    throw new NotSupportedException($"{lifetime} is invalid.");
            }
        }

        private object Instantiate(IRegistration registration)
        {
            var ownership = registration.Ownership;

            switch (ownership)
            {
                case Ownership.Internal:
                    var instance = registration.Instantiate(this);

                    if (instance is IDisposable disposable)
                    {
                        engine.Bind(disposable);
                    }

                    return instance;
                case Ownership.External:
                    return registration.Instantiate(this);
                default:
                    throw new NotSupportedException($"{ownership} is invalid.");
            }
        }

        public object Resolve(Type type, IScopedResolver child)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            if (engine.Find(type, out var registration))
            {
                var lifetime = registration.Lifetime;

                switch (lifetime)
                {
                    case Lifetime.Global:
                        return ResolveInternally(registration);
                    case Lifetime.Local:
                    case Lifetime.Temporal:
                        return child.Resolve(registration);
                    default:
                        throw new NotSupportedException($"{lifetime} is invalid.");
                }
            }

            if (parent != null)
            {
                return parent.Resolve(type, child);
            }

            throw new RagnarokNotRegisteredException(type);
        }

        public object Resolve(IRegistration registration)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            if (parent == null)
            {
                return ResolveInternally(registration);
            }

            var lifetime = registration.Lifetime;

            switch (lifetime)
            {
                case Lifetime.Global:
                {
                    if (engine.Have(registration))
                    {
                        return ResolveInternally(registration);
                    }

                    return parent.Resolve(registration);
                }
                case Lifetime.Local:
                case Lifetime.Temporal:
                    return ResolveInternally(registration);
                default:
                    throw new NotSupportedException($"{lifetime} is invalid.");
            }
        }

        public IEnumerable<IRegistration> ResolveAll(Type type)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            if (parent == null)
            {
                if (!engine.Find(type, out var registration))
                {
                    return Array.Empty<IRegistration>();
                }

                return new[] { registration };
            }
            else
            {
                if (!engine.Find(type, out var registration))
                {
                    return parent.ResolveAll(type);
                }

                return parent.ResolveAll(type).Append(registration);
            }
        }

        public IScopedResolverContext CreateContext()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            return new ScopedResolverContext(engineBuilder, this);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScopedResolver));
            }

            engine.Dispose();

            isDisposed = false;
        }
    }
}
