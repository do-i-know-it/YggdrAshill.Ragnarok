using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class AssignedTypeCollection
    {
        public Type ImplementedType { get; }

        public AssignedTypeCollection(Type implementedType)
        {
            ImplementedType = implementedType;
        }

        private readonly List<Type> assignedTypeList = new List<Type>();
        public IReadOnlyList<Type> AssignedTypeList => assignedTypeList;

        public void Add(Type type)
        {
            if (!type.IsAssignableFrom(ImplementedType))
            {
                throw new Exception($"{ImplementedType} is not assignable from {type}");
            }

            AddToAssignedTypeList(type);
        }

        public void AddSelf()
        {
            AddToAssignedTypeList(ImplementedType);
        }

        public void AddImplementedInterfaces()
        {
            foreach (var interfaceType in ImplementedType.GetInterfaces())
            {
                AddToAssignedTypeList(interfaceType);
            }
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
