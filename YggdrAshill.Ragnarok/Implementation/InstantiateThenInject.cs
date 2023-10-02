namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateThenInject : IInstantiation
    {
        private readonly IInstantiation instantiation;
        private readonly IInjection injection;

        public InstantiateThenInject(IInstantiation instantiation, IInjection injection)
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
