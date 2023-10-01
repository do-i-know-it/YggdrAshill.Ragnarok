using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Parameter : IParameter
    {
        private readonly Func<Argument, bool> detection;
        private readonly Func<object> instantiation;

        public Parameter(string name, Func<object> instantiation)
        {
            detection = argument => argument.Name == name;
            this.instantiation = instantiation;
        }

        public Parameter(string name, object instance) : this(name, () => instance)
        {

        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (!detection.Invoke(argument))
            {
                return false;
            }

            instance = instantiation.Invoke();

            return true;
        }
    }

    internal sealed class Parameter<T> : IParameter
        where T: notnull
    {
        private readonly Func<Argument, bool> detection;
        private readonly Func<T> instantiation;

        public Parameter(Func<T> instantiation)
        {
            var type = typeof(T);
            detection = argument => argument.Type == type;
            this.instantiation = instantiation;
        }

        public Parameter(T instance) : this (() => instance)
        {

        }

        public Parameter(string name, Func<T> instantiation)
        {
            detection = argument => argument.Type == typeof(T) && argument.Name == name;
            this.instantiation = instantiation;
        }

        public Parameter(string name, T instance) : this(name, () => instance)
        {

        }

        public bool CanResolve(Argument argument, out object instance)
        {
            instance = default!;

            if (!detection.Invoke(argument))
            {
                return false;
            }

            instance = instantiation.Invoke();

            return true;
        }
    }
}
