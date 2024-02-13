using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Invocation<T> : IInvocation<T>
        where T : notnull
    {
        private readonly Action<T> onInvoked;

        public Invocation(Action<T> onInvoked)
        {
            this.onInvoked = onInvoked;
        }

        public void Invoke(T instance)
        {
            onInvoked.Invoke(instance);
        }
    }
}
