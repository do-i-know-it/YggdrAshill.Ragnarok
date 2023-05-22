using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok
{
    internal sealed class AfterAnyTypeAssigned :
        IAfterAnyTypeAssigned
    {
        private readonly AssignedTypeCollection collection;

        public AfterAnyTypeAssigned(AssignedTypeCollection collection)
        {
            this.collection = collection;
        }

        public IAfterAnyTypeAssigned And<T>()
            where T : notnull
        {
            collection.Add(typeof(T));

            return this;
        }

        public void AndSelf()
        {
            collection.AddSelf();
        }
    }
}
