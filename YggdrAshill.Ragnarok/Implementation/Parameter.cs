using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Parameter<T> :
        IParameter
        where T: notnull
    {
        private readonly Func<object> createInstance;

        public string Name { get; }
        public Type Type { get; } = typeof(T);
        public object Instance => createInstance.Invoke();

        public Parameter(string name, Func<object> createInstance)
        {
            Name = name;
            this.createInstance = createInstance;
        }

        public Parameter(string name, T instance) : this(name, () => instance)
        {

        }
    }
}
