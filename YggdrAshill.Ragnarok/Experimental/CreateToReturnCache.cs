using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateToReturnCache<T> : ICreation<T>
        where T : notnull
    {
        private readonly Lazy<T> cache;

        public CreateToReturnCache(Func<T> creation)
        {
            cache = new Lazy<T>(creation);
        }

        public T Create()
        {
            return cache.Value;
        }
    }
}
