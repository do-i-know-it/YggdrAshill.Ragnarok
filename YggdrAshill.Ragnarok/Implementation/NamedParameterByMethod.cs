using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class NamedParameterByMethod<T> : IParameter
        where T : notnull
    {
        private static Type ParameterType { get; } = typeof(T);

        private readonly string name;
        private readonly Lazy<T> instanceCache;

        public NamedParameterByMethod(string name, Func<T> instantiation)
        {
            this.name = name;
            instanceCache = new Lazy<T>(instantiation);
        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (argument.Type != ParameterType || argument.Name != name)
            {
                return false;
            }

            instance = instanceCache.Value;

            return true;
        }
    }
}
