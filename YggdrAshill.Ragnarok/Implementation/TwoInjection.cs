namespace YggdrAshill.Ragnarok
{
    internal sealed class TwoInjection : IInjection
    {
        private readonly IInjection first;
        private readonly IInjection second;

        public TwoInjection(IInjection first, IInjection second)
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
