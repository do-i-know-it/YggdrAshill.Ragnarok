namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateAntInject : IInstantiation
    {
        private readonly IInstantiation instantiation;
        private readonly IInjection injection;

        public InstantiateAntInject(IInstantiation instantiation, IInjection injection)
        {
            this.instantiation = instantiation;
            this.injection = injection;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            var instance = instantiation.Instantiate(resolver);

            injection.Inject(resolver, instance);

            return instance;
        }
    }
}
