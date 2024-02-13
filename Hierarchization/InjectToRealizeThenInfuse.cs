namespace YggdrAshill.Ragnarok
{
    internal sealed class InjectToRealizeThenInfuse : IInjection
    {
        private readonly IRealization realization;
        private readonly IInfusion infusion;

        public InjectToRealizeThenInfuse(IRealization realization, IInfusion infusion)
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
