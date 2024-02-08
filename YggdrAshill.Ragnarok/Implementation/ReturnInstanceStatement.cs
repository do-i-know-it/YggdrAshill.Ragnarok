using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnInstanceStatement : IStatement
    {
        public TypeAssignmentSource Source { get; }
        public IInstantiation Instantiation { get; }

        public ReturnInstanceStatement(object instance)
        {
            Source = new TypeAssignmentSource(instance.GetType());
            Instantiation = new ReturnInstance(instance);
        }

        public Type ImplementedType => Source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Global;

        public Ownership Ownership => Ownership.External;
    }
}
