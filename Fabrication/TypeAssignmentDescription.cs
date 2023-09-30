using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public sealed class TypeAssignmentDescription : IDescriptionV2
    {
        private readonly TypeAssignment assignment;

        public IInstantiationV2 Instantiation { get; }

        public TypeAssignmentDescription(object instance)
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
