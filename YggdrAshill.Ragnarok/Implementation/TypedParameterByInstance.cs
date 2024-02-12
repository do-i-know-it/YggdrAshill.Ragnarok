using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class TypedParameterByInstance<T> : IParameter
        where T : notnull
    {
        private static Type ParameterType { get; } = typeof(T);

        private readonly T instanceCache;

        public TypedParameterByInstance(T instanceCache)
        {
            this.instanceCache = instanceCache;
        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (argument.Type != ParameterType)
            {
                return false;
            }

            instance = instanceCache;

            return true;
        }
    }
}
