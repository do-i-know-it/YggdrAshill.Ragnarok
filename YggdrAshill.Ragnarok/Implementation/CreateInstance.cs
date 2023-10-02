using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateInstance<T> : IInstantiation
        where T : notnull
    {
        private readonly Func<T> onInstantiated;

        public CreateInstance(Func<T> onInstantiated)
        {
            this.onInstantiated = onInstantiated;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return onInstantiated.Invoke();
        }
    }
}
