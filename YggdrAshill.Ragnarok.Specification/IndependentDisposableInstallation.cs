namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class IndependentDisposableInstallation : IInstallation
    {
        public void Install(IObjectContainer container)
        {
            container.Register<IndependentDisposable>(Lifetime.Global);
        }
    }
}
