using System;

namespace YggdrAshill.Ragnarok.Materialization
{
    public sealed class Argument
    {
        public string Name { get; }
        public Type Type { get; }

        public Argument(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}
