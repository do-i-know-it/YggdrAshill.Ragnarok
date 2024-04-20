namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class DisposableOutputInstallation : IInstallation
    {
        public Lifetime Lifetime { get; }

        public DisposableOutputInstallation(Lifetime lifetime)
        {
            Lifetime = lifetime;
        }

        public void Install(IObjectContainer container)
        {
            container.Register<DisposableOutput>(Lifetime);
        }
    }
}
