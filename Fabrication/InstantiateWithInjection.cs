namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateWithInjection : IInstantiationV2
    {
        private readonly IInstantiationV2 instantiation;
        private readonly IInjectionV2 injection;

        public InstantiateWithInjection(IInstantiationV2 instantiation, IInjectionV2 injection)
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
