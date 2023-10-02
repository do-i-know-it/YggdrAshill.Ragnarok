using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class TypeAssignmentStatement : ITypeAssignment, IStatement
    {
        private readonly TypeAssignmentSource assignment;
        private readonly Lazy<IInstantiation> instantiation;

        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public TypeAssignmentStatement(Type type, Lifetime lifetime, Ownership ownership, Func<IInstantiation> createInstantiation)
        {
            assignment = new TypeAssignmentSource(type);
            Lifetime = lifetime;
            Ownership = ownership;
            instantiation = new Lazy<IInstantiation>(createInstantiation);
        }

        public Type ImplementedType => assignment.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => assignment.AssignedTypeList;

        public IInstantiation Instantiation => instantiation.Value;

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
