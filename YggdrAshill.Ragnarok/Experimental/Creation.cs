using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Creation<T> : ICreation<T>
        where T : notnull
    {
        private readonly Func<T> onCreated;

        public Creation(Func<T> onCreated)
        {
            this.onCreated = onCreated;
        }

        public T Create()
        {
            return onCreated.Invoke();
        }
    }
}
