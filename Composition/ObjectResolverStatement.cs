using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ObjectResolverStatement : IStatement, IInstantiation
    {
        public static ObjectResolverStatement Instance { get; } = new();

        private ObjectResolverStatement()
        {

        }

        public Type ImplementedType { get; } = typeof(IObjectResolver);
        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();
        public Lifetime Lifetime => Lifetime.Temporal;
        public Ownership Ownership => Ownership.External;
        public IInstantiation Instantiation => this;

        public object Instantiate(IObjectResolver resolver)
        {
            return resolver;
        }
    }
}
