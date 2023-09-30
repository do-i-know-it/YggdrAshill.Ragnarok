namespace YggdrAshill.Ragnarok
{
    internal sealed class InjectWithThreeInjection : IInjectionV2
    {
        private readonly IInjectionV2 first;
        private readonly IInjectionV2 second;
        private readonly IInjectionV2 third;

        public InjectWithThreeInjection(IInjectionV2 first, IInjectionV2 second, IInjectionV2 third)
        {
            this.first = first;
            this.second = second;
            this.third = third;
        }

        public void Inject(IObjectResolver resolver, object instance)
        {
            first.Inject(resolver, instance);
            second.Inject(resolver, instance);
            third.Inject(resolver, instance);
        }
    }
}
