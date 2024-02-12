using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class TypedParameterByMethod<T> : IParameter
        where T : notnull
    {
        private static Type ParameterType { get; } = typeof(T);

        private readonly Lazy<T> instanceCache;

        public TypedParameterByMethod(Func<T> instantiation)
        {
            instanceCache = new Lazy<T>(instantiation);
        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (argument.Type != ParameterType)
            {
                return false;
            }

            instance = instanceCache.Value;

            return true;
        }
    }
}
