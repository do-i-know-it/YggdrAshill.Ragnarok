using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateAndInjectIntoInstance :
        IInstantiation
    {
        private readonly IInstantiation instantiation;
        private readonly IInjection injection;

        public InstantiateAndInjectIntoInstance(IInstantiation instantiation, IInjection injection)
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
