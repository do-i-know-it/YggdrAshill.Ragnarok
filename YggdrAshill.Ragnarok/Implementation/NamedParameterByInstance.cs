using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class NamedParameterByInstance<T> : IParameter
        where T : notnull
    {
        private static Type ParameterType { get; } = typeof(T);

        private readonly string name;
        private readonly T instanceCache;

        public NamedParameterByInstance(string name, T instanceCache)
        {
            this.name = name;
            this.instanceCache = instanceCache;
        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (argument.Type != ParameterType || argument.Name != name)
            {
                return false;
            }

            instance = instanceCache;

            return true;
        }
    }
}
