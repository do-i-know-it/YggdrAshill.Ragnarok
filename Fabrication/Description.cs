using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Description :
        IDescription
    {
        private readonly IStatement statement;

        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public Description(IStatement statement, Lifetime lifetime, Ownership ownership)
        {
            this.statement = statement;
            Lifetime = lifetime;
            Ownership = ownership;
        }

        public Type ImplementedType => statement.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => statement.AssignedTypeList;
        public IInstantiation Instantiation => statement.Instantiation;
    }
}
