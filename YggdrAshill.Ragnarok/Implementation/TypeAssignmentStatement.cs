using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class TypeAssignmentStatement : ITypeAssignment, IStatement
    {
        private readonly Func<IInstantiation> createInstantiation;

        public Type ImplementedType { get; }
        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }

        public TypeAssignmentStatement(Type type, Lifetime lifetime, Ownership ownership, Func<IInstantiation> createInstantiation)
        {
            this.createInstantiation = createInstantiation;
            ImplementedType = type;
            Lifetime = lifetime;
            Ownership = ownership;
        }

        public TypeAssignmentStatement(object instance)
            : this(instance.GetType(), Lifetime.Global, Ownership.External, () => new InstantiateToReturnInstance(instance))
        {

        }

        private readonly List<Type> assignedTypeList = new();
        public IReadOnlyList<Type> AssignedTypeList => assignedTypeList;

        public IInstantiation Instantiation => createInstantiation.Invoke();

        public void AsOwnSelf()
        {
            AddToAssignedTypeList(ImplementedType);
        }

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            if (!inheritedType.IsAssignableFrom(ImplementedType))
            {
                throw new ArgumentException($"{ImplementedType} is not assignable from {inheritedType}.");
            }

            AddToAssignedTypeList(inheritedType);

            return this;
        }

        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            foreach (var interfaceType in ImplementedType.GetInterfaces())
            {
                AddToAssignedTypeList(interfaceType);
            }

            return this;
        }

        private void AddToAssignedTypeList(Type type)
        {
            if (assignedTypeList.Contains(type))
            {
                return;
            }

            assignedTypeList.Add(type);
        }
    }
}
