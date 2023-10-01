namespace YggdrAshill.Ragnarok
{
    internal sealed class ActivateToInstantiateWithoutParameterList : IInstantiation
    {
        private readonly IActivation activation;

        public ActivateToInstantiateWithoutParameterList(IActivation activation)
        {
            this.activation = activation;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            var argumentList = activation.ArgumentList;

            // TODO: object pooling.
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = resolver.Resolve(argument.Type);
            }

            return activation.Activate(instanceList);
        }
    }
}
