using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class ReturnInstanceStatement : IStatement
    {
        private readonly TypeAssignmentSource source;

        public IInstantiation Instantiation { get; }

        public ReturnInstanceStatement(object instance)
        {
            source = new TypeAssignmentSource(instance.GetType());
            Instantiation = new ReturnInstance(instance);
        }

        public ITypeAssignment TypeAssignment => source;

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Global;

        public Ownership Ownership => Ownership.External;
    }
}
