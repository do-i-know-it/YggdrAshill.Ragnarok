using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class AssignAnyTypeToCompose :
        IAssignAnyType,
        IComposition
    {
        private readonly Lifetime lifetime;
        private readonly Ownership ownership;
        private readonly IInstantiation instantiation;
        private readonly AssignedTypeCollection collection;

        public AssignAnyTypeToCompose(AssignedTypeCollection collection, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
        {
            this.lifetime = lifetime;
            this.ownership = ownership;
            this.instantiation = instantiation;
            this.collection = collection;
        }

        public AssignAnyTypeToCompose(Type implementedType, Lifetime lifetime, Ownership ownership, IInstantiation instantiation)
            :this(new AssignedTypeCollection(implementedType), lifetime, ownership, instantiation)
        {

        }

        public IDescription Compose()
        {
            return new Description(collection.ImplementedType, collection.AssignedTypeList, lifetime, ownership, instantiation);
        }

        public IAfterAnyTypeAssigned As<T>()
            where T : notnull
        {
            collection.Add(typeof(T));

            return new AfterAnyTypeAssigned(collection);
        }

        public IAfterImplementedTypeAssigned AsSelf()
        {
            collection.AddSelf();

            return new AfterImplementedTypeAssigned(collection);
        }

        public IAfterImplementedInterfacesAssigned AsImplementedInterfaces()
        {
            collection.AddImplementedInterfaces();

            return new AfterImplementedInterfacesAssigned(collection);
        }
    }
}
