namespace YggdrAshill.Ragnarok
{
    internal sealed class InjectWithTwoInjection : IInjectionV2
    {
        private readonly IInjectionV2 first;
        private readonly IInjectionV2 second;

        public InjectWithTwoInjection(IInjectionV2 first, IInjectionV2 second)
        {
            this.first = first;
            this.second = second;
        }

        public void Inject(IObjectResolver resolver, object instance)
        {
            first.Inject(resolver, instance);
            second.Inject(resolver, instance);
        }
    }
}
