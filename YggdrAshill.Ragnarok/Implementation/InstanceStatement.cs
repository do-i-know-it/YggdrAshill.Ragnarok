using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class InstanceStatement : ITypeAssignment, IStatement
    {
        private readonly TypeAssignmentSource assignment;

        public InstanceStatement(object instance)
        {
            assignment = new TypeAssignmentSource(instance.GetType());
            Instantiation = new InstantiateToReturnInstance(instance);
        }

        public Type ImplementedType => assignment.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => assignment.AssignedTypeList;

        public Lifetime Lifetime => Lifetime.Global;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation { get; }

        public void AsOwnSelf()
        {
            assignment.AssignOwnType();
        }

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            assignment.Assign(inheritedType);

            return this;
        }

        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            assignment.AssignAllInterfaces();

            return this;
        }
    }
}
