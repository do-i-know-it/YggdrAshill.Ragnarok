namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseToInjectWithoutParameterList : IInjection
    {
        private readonly IInfusion infusion;

        public InfuseToInjectWithoutParameterList(IInfusion infusion)
        {
            this.infusion = infusion;
        }

        public void Inject(IObjectResolver resolver, object instance)
        {
            var argumentList = infusion.ArgumentList;

            // TODO: object pooling.
            var instanceList = new object[argumentList.Count];

            for (var index = 0; index < argumentList.Count; index++)
            {
                var argument = argumentList[index];

                instanceList[index] = resolver.Resolve(argument.Type);
            }

            infusion.Infuse(instance, instanceList);
        }
    }
}
