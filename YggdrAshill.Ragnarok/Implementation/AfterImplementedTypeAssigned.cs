using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok
{
    internal sealed class AfterImplementedTypeAssigned :
        IAfterImplementedTypeAssigned
    {
        private readonly AssignedTypeCollection collection;

        public AfterImplementedTypeAssigned(AssignedTypeCollection collection)
        {
            this.collection = collection;
        }

        public IAfterImplementedTypeAssigned As<T>()
            where T : notnull
        {
            collection.Add(typeof(T));;

            return this;
        }

        public void AsImplementedInterfaces()
        {
            collection.AddImplementedInterfaces();
        }
    }
}
