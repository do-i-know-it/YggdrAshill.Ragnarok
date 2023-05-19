using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class AssignAnyTypeToCompose :
        IAssignAnyType,
        IComposition
    {
        private readonly Type implementedType;
        private readonly Lifetime lifetime;
        private readonly Ownership ownership;
        private readonly IInstantiation instantiation;

        public AssignAnyTypeToCompose(Type implementedType, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
        {
            this.implementedType = implementedType;
            this.lifetime = lifetime;
            this.ownership = ownership;
            this.instantiation = instantiation;
        }

        private readonly List<Type> assignedTypeList = new List<Type>();

        public IDescription Compose()
        {
            return new Description(implementedType, assignedTypeList, lifetime, ownership, instantiation);
        }

        public IAfterAnyTypeAssigned As<T>()
            where T : notnull
        {
            AddType<T>();

            return new AfterAnyTypeAssigned(this);
        }
        private void AddType<T>()
            where T : notnull
        {
            var type = typeof(T);

            if (!type.IsAssignableFrom(implementedType))
            {
                throw new Exception($"{implementedType} is not assignable from {type}");
            }

            AddToAssignedTypeList(type);
        }

        public IAfterImplementedTypeAssigned AsSelf()
        {
            AddImplementedType();

            return new AfterImplementedTypeAssigned(this);
        }
        private void AddImplementedType()
        {
            AddToAssignedTypeList(implementedType);
        }

        public IAfterImplementedInterfacesAssigned AsImplementedInterfaces()
        {
            AddImplementedInterfaces();

            return new AfterImplementedInterfacesAssigned(this);
        }
        private void AddImplementedInterfaces()
        {
            foreach (var interfaceType in implementedType.GetInterfaces())
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

        private sealed class Description :
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

        private sealed class AfterAnyTypeAssigned :
            IAfterAnyTypeAssigned
        {
            private readonly AssignAnyTypeToCompose assignAnyTypeToCompose;

            public AfterAnyTypeAssigned(AssignAnyTypeToCompose assignAnyTypeToCompose)
            {
                this.assignAnyTypeToCompose = assignAnyTypeToCompose;
            }

            public IAfterAnyTypeAssigned As<T>()
                where T : notnull
            {
                return And<T>();
            }

            public IAfterAnyTypeAssigned And<T>()
                where T : notnull
            {
                assignAnyTypeToCompose.AddType<T>();

                return this;
            }

            public void AsSelf()
            {
                AndSelf();
            }

            public void AndSelf()
            {
                assignAnyTypeToCompose.AddImplementedType();
            }
        }
        private sealed class AfterImplementedTypeAssigned :
            IAfterImplementedTypeAssigned
        {
            private readonly AssignAnyTypeToCompose assignAnyTypeToCompose;

            public AfterImplementedTypeAssigned(AssignAnyTypeToCompose assignAnyTypeToCompose)
            {
                this.assignAnyTypeToCompose = assignAnyTypeToCompose;
            }

            public IAfterImplementedTypeAssigned As<T>()
                where T : notnull
            {
                assignAnyTypeToCompose.AddType<T>();

                return this;
            }

            public void AsImplementedInterfaces()
            {
                assignAnyTypeToCompose.AddImplementedInterfaces();
            }
        }
        private sealed class AfterImplementedInterfacesAssigned :
            IAfterImplementedInterfacesAssigned
        {
            private readonly AssignAnyTypeToCompose assignAnyTypeToCompose;

            public AfterImplementedInterfacesAssigned(AssignAnyTypeToCompose assignAnyTypeToCompose)
            {
                this.assignAnyTypeToCompose = assignAnyTypeToCompose;
            }

            public void AsSelf()
            {
                AndSelf();
            }

            public void AndSelf()
            {
                assignAnyTypeToCompose.AddImplementedType();
            }
        }
    }
}
