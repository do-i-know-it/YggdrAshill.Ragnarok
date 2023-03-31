using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Hierarchization
{
    /// <summary>
    /// Implementation of <see cref="IScope"/> using <see cref="IScopedResolver"/>.
    /// </summary>
    internal sealed class Scope :
        IScope
    {
        private readonly IScopedResolver scopedResolver;
        private readonly ICollection<IDisposable> disposableList;

        public Scope(IScopedResolver scopedResolver, ICollection<IDisposable> disposableList)
        {
            this.scopedResolver = scopedResolver;
            this.disposableList = disposableList;
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

            foreach (var disposable in disposableList)
            {
                disposable.Dispose();
            }

            disposableList.Clear();

            isDisposed = true;
        }
    }
}
