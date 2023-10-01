using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Parameter : IParameter
    {
        private readonly Func<Argument, bool> detection;
        private readonly Func<object> instantiation;

        private Parameter(Func<Argument, bool> detection, Func<object> instantiation)
        {
            this.detection = detection;
            this.instantiation = instantiation;
        }

        public Parameter(string name, Func<object> instantiation)
            : this(argument => argument.Name == name, instantiation)
        {

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

        private Parameter(Func<Argument, bool> detection, Func<T> instantiation)
        {
            this.detection = detection;
            this.instantiation = instantiation;
        }

        public Parameter(Func<T> instantiation)
            : this (argument => argument.Type == typeof(T), instantiation)
        {

        }

        public Parameter(T instance) : this (() => instance)
        {

        }

        public Parameter(string name, Func<T> instantiation)
            : this (argument => argument.Type == typeof(T) && argument.Name == name, instantiation)
        {

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
