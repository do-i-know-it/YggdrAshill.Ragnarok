using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class TypedParameter<T> : IParameter
        where T : notnull
    {
        private static Type ParameterType { get; } = typeof(T);

        private readonly ICreation<T> creation;

        public TypedParameter(Func<T> creation) : this(new CreateToReturnCache<T>(creation))
        {

        }

        public TypedParameter(T instance) : this(new CreateToReturnInstance<T>(instance))
        {

        }

        private TypedParameter(ICreation<T> creation)
        {
            this.creation = creation;
        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (argument.Type != ParameterType)
            {
                return false;
            }

            instance = creation.Create();

            return true;
        }
    }
}
