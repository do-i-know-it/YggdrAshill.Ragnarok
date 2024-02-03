namespace YggdrAshill.Ragnarok
{
    internal sealed class RealizeThenInfuseToInject : IInjection
    {
        private readonly IRealization realization;
        private readonly IInfusionV2 infusion;

        public RealizeThenInfuseToInject(IRealization realization, IInfusionV2 infusion)
        {
            this.realization = realization;
            this.infusion = infusion;
        }

        public void Inject(IObjectResolver resolver, object instance)
        {
            var parameterList = realization.Realize(resolver);

            infusion.Infuse(instance, parameterList);
        }
    }
}
