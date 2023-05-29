namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateWithInjection :
        IInstantiation
    {
        private readonly IInstantiation instantiation;
        private readonly IInjection injection;

        public InstantiateWithInjection(IInstantiation instantiation, IInjection injection)
        {
            this.instantiation = instantiation;
            this.injection = injection;
        }

        public object Instantiate(IResolver resolver)
        {
            var instance = instantiation.Instantiate(resolver);

            injection.Inject(resolver, instance);

            return instance;
        }
    }
}
