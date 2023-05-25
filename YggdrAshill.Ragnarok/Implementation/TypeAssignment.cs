using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class TypeAssignment :
        ITypeAssignment
    {
        public Type ImplementedType { get; }

        public TypeAssignment(Type implementedType)
        {
            ImplementedType = implementedType;
        }

        private readonly List<Type> assignedTypeList = new List<Type>();
        public IReadOnlyList<Type> AssignedTypeList => assignedTypeList;

        public void AsSelf()
        {
            AddToAssignedTypeList(ImplementedType);
        }

        public IAssignImplementedInterface As(Type implementedInterface)
        {
            if (!implementedInterface.IsAssignableFrom(ImplementedType))
            {
                throw new Exception($"{ImplementedType} is not assignable from {implementedInterface}.");
            }

            AddToAssignedTypeList(implementedInterface);

            return this;
        }

        public IAssignImplementedType AsImplementedInterfaces()
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
