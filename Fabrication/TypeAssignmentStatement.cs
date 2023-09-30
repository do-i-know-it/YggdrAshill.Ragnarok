using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class TypeAssignmentStatement : IStatement
    {
        private readonly TypeAssignment assignment;

        public IInstantiation Instantiation { get; }

        public TypeAssignmentStatement(object instance)
        {
            assignment = new TypeAssignment(instance.GetType());
            Instantiation = new InstantiateToReturnInstance(instance);
        }

        public ITypeAssignment Assignment => assignment;
        public Type ImplementedType => assignment.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => assignment.AssignedTypeList;
        public Lifetime Lifetime => Lifetime.Global;
        public Ownership Ownership => Ownership.External;
    }
}
