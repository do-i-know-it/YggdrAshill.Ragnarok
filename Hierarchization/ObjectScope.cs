using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectScope : IObjectScope
    {
        private readonly IScopedResolverV2 resolver;

        public ObjectScope(IScopedResolverV2 resolver)
        {
            this.resolver = resolver;
        }

        private bool isDisposed;

        public IObjectResolver Resolver
        {
            get
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(nameof(IScope));
                }

                return resolver;
            }
        }

        public IObjectContext CreateContext()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScope));
            }

            var resolverBuilder = resolver.CreateBuilder();

            return new ObjectContext(resolverBuilder);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScope));
            }

            resolver.Dispose();

            isDisposed = true;
        }
    }
}
