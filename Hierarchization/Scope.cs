using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Scope :
        IScope
    {
        private readonly IScopedResolver scopedResolver;

        public Scope(IScopedResolver scopedResolver)
        {
            this.scopedResolver = scopedResolver;
        }

        private bool isDisposed;

        public IResolver Resolver
        {
            get
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(nameof(IScope));
                }

                return scopedResolver;
            }
        }

        public IContext CreateContext()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScope));
            }

            var context = scopedResolver.CreateContext();

            return new Context(context);
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(IScope));
            }

            scopedResolver.Dispose();

            isDisposed = true;
        }
    }
}
