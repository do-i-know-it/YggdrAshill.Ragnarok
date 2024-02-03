namespace YggdrAshill.Ragnarok
{
    internal sealed class RealizeThenInfuseToInject : IInjection
    {
        private readonly IRealization realization;
        private readonly IInfusion infusion;

        public RealizeThenInfuseToInject(IRealization realization, IInfusion infusion)
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
