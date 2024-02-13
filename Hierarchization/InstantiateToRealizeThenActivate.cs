namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToRealizeThenActivate : IInstantiation
    {
        private readonly IRealization realization;
        private readonly IActivation activation;

        public InstantiateToRealizeThenActivate(IRealization realization, IActivation activation)
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
