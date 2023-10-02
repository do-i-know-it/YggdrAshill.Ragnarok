using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class TypeAssignmentSource : ITypeAssignment
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

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            if (!inheritedType.IsAssignableFrom(ImplementedType))
            {
                throw new ArgumentException($"{ImplementedType} is not assignable from {inheritedType}.");
            }

            AddToAssignedTypeList(inheritedType);

            return this;
        }

        public void AsOwnSelf()
        {
            AddToAssignedTypeList(ImplementedType);
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
