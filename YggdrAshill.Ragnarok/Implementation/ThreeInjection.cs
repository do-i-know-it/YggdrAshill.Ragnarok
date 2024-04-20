namespace YggdrAshill.Ragnarok
{
    internal sealed class ThreeInjection : IInjection
    {
        private readonly IInjection first;
        private readonly IInjection second;
        private readonly IInjection third;

        public ThreeInjection(IInjection first, IInjection second, IInjection third)
        {
            this.first = first;
            this.second = second;
            this.third = third;
        }

        public void Inject(IObjectResolver resolver, ref object instance)
        {
            first.Inject(resolver, ref instance);
            second.Inject(resolver, ref instance);
            third.Inject(resolver, ref instance);
        }
    }
}
