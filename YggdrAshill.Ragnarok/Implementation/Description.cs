using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Description :
        IDescription
    {
        public Description(
            Type implementedType, IReadOnlyList<Type> assignedTypeList,
            Lifetime lifetime, Ownership ownership,
            IInstantiation instantiation)
        {
            ImplementedType = implementedType;
            AssignedTypeList = assignedTypeList;
            Lifetime = lifetime;
            Ownership = ownership;
            Instantiation = instantiation;
        }

        public Type ImplementedType { get; }
        public IReadOnlyList<Type> AssignedTypeList { get; }
        public Lifetime Lifetime { get; }
        public Ownership Ownership { get; }
        public IInstantiation Instantiation { get; }
    }
}
