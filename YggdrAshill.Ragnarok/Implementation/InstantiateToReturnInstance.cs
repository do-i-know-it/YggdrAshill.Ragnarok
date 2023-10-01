namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToReturnInstance : IInstantiation
    {
        private readonly object instance;

        public InstantiateToReturnInstance(object instance)
        {
            this.instance = instance;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return instance;
        }
    }
}
