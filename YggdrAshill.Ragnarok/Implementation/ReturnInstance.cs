namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnInstance : IInstantiation
    {
        private readonly object instance;

        public ReturnInstance(object instance)
        {
            this.instance = instance;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return instance;
        }
    }
}
