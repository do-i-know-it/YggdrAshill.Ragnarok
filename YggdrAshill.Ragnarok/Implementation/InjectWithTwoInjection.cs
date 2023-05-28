namespace YggdrAshill.Ragnarok
{
    internal sealed class InjectWithTwoInjection :
        IInjection
    {
        private readonly IInjection first;
        private readonly IInjection second;

        public InjectWithTwoInjection(IInjection first, IInjection second)
        {
            this.first = first;
            this.second = second;
        }

        public void Inject(IResolver resolver, object instance)
        {
            first.Inject(resolver, instance);
            second.Inject(resolver, instance);
        }
    }
}
