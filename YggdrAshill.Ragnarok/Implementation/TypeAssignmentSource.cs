using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class TypeAssignmentSource
    {
        public Type ImplementedType { get; }

        public TypeAssignmentSource(Type type)
        {
            ImplementedType = type;
        }

        private List<Type>? assignedTypeList;
        public IReadOnlyList<Type> AssignedTypeList
        {
            get
            {
                if (assignedTypeList == null)
                {
                    return Array.Empty<Type>();
                }

                return assignedTypeList;
            }
        }

        public void Assign(Type inheritedType)
        {
            if (!inheritedType.IsAssignableFrom(ImplementedType))
            {
                throw new ArgumentException($"{ImplementedType} is not assignable from {inheritedType}.");
            }

            AddToAssignedTypeList(inheritedType);
        }

        public void AssignOwnType()
        {
            AddToAssignedTypeList(ImplementedType);
        }

        public void AssignAllInterfaces()
        {
            foreach (var interfaceType in ImplementedType.GetInterfaces())
            {
                AddToAssignedTypeList(interfaceType);
            }
        }

        private void AddToAssignedTypeList(Type type)
        {
            if (assignedTypeList == null)
            {
                assignedTypeList = new List<Type>();
            }

            if (assignedTypeList.Contains(type))
            {
                return;
            }

            assignedTypeList.Add(type);
        }
    }
}
