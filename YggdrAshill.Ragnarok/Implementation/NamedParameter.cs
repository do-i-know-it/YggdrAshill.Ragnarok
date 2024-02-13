using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class NamedParameter<T> : IParameter
        where T : notnull
    {
        private static Type ParameterType { get; } = typeof(T);

        private readonly string name;
        private readonly ICreation<T> creation;

        public NamedParameter(string name, Func<T> creation) : this(name, new CreateToReturnCache<T>(creation))
        {

        }

        public NamedParameter(string name, T instance) : this(name, new CreateToReturnInstance<T>(instance))
        {

        }

        private NamedParameter(string name, ICreation<T> creation)
        {
            this.name = name;
            this.creation = creation;
        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (argument.Type != ParameterType || argument.Name != name)
            {
                return false;
            }

            instance = creation.Create();

            return true;
        }
    }
}
