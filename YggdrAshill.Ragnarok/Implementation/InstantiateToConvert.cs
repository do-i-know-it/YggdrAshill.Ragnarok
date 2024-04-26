using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToConvert<TInput, TOutput> : IInstantiation
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly Func<TInput, TOutput> onConverted;

        public InstantiateToConvert(Func<TInput, TOutput> onConverted)
        {
            this.onConverted = onConverted;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            var input = resolver.Resolve<TInput>();
            return onConverted.Invoke(input);
        }
    }
}
