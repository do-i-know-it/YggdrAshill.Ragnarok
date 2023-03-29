using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class FuncParameter<T> :
        IParameter
        where T: notnull
    {
        private readonly Func<T> instantiation;

        public Type Type { get; } = typeof(T);
        public string Name { get; }

        public FuncParameter(string name, Func<T> instantiation)
        {
            Name = name;
            this.instantiation = instantiation;
        }

        public object Instance => instantiation.Invoke();
    }
    public sealed class Parameter<T> :
        IParameter
        where T: notnull
    {
        public Type Type { get; } = typeof(T);
        public string Name { get; }
        public object Instance { get; }

        public Parameter(string name, T instance)
        {
            Name = name;
            Instance = instance;
        }
    }
}
