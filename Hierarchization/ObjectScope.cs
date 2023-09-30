using System;

namespace YggdrAshill.Ragnarok.Hierarchization
{
    internal sealed class ObjectScope : IObjectScope
    {
        private readonly IScopedResolver resolver;

        public ObjectScope(IScopedResolver resolver)
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
                    throw new ObjectDisposedException(nameof(IObjectScope));
                }

                return resolver;
            }
        }

        public IObjectContext CreateContext()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IObjectScope));
            }

            var resolverBuilder = resolver.CreateBuilder();

            return new ObjectContext(resolverBuilder);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IObjectScope));
            }

            resolver.Dispose();

            isDisposed = true;
        }
    }
}
