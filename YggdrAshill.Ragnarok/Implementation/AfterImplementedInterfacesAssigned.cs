using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok
{
    internal sealed class AfterImplementedInterfacesAssigned :
        IAfterImplementedInterfacesAssigned
    {
        private readonly AssignedTypeCollection collection;

        public AfterImplementedInterfacesAssigned(AssignedTypeCollection collection)
        {
            this.collection = collection;
        }

        public void AsSelf()
        {
            AndSelf();
        }

        public void AndSelf()
        {
            collection.AddSelf();
        }
    }
}
