using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class TypeAssignmentStatement :
        ITypeAssignment,
        IStatement
    {
        private readonly TypeAssignment assignment;

        public IInstantiation Instantiation { get; }

        public TypeAssignmentStatement(Type implementedType, IInstantiation instantiation)
        {
            assignment = new TypeAssignment(implementedType);
            Instantiation = instantiation;
        }

        public void AsSelf()
        {
            assignment.AsSelf();
        }

        public IAssignImplementedInterface As(Type implementedInterface)
        {
            return assignment.As(implementedInterface);
        }

        public IAssignImplementedType AsImplementedInterfaces()
        {
            return assignment.AsImplementedInterfaces();
        }

        public Type ImplementedType => assignment.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => assignment.AssignedTypeList;
    }
}
