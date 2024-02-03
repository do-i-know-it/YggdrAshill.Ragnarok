namespace YggdrAshill.Ragnarok
{
    internal sealed class RealizeThenActivateToInstantiate : IInstantiation
    {
        private readonly IRealization realization;
        private readonly IActivationV2 activation;

        public RealizeThenActivateToInstantiate(IRealization realization, IActivationV2 activation)
        {
            this.realization = realization;
            this.activation = activation;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            var parameterList = realization.Realize(resolver);

            return activation.Activate(parameterList);
        }
    }
}
