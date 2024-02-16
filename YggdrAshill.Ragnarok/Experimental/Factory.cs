using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Factory<T> : IFactory<T>
        where T : notnull
    {
        private readonly Func<T> onCreated;

        public Factory(Func<T> onCreated)
        {
            this.onCreated = onCreated;
        }

        public T Create()
        {
            return onCreated.Invoke();
        }
    }

    internal sealed class Factory<TInput, TOutput> : IFactory<TInput, TOutput>
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly Func<TInput, TOutput> onCreated;

        public Factory(Func<TInput, TOutput> onCreated)
        {
            this.onCreated = onCreated;
        }

        public TOutput Create(TInput input)
        {
            return onCreated.Invoke(input);
        }
    }
}
