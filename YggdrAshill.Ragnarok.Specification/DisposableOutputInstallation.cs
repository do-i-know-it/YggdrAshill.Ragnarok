namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class DisposableOutputInstallation : IInstallation
    {
        public void Install(IObjectContainer container)
        {
            container.Register<DisposableOutput>(Lifetime.Temporal);
        }
    }
}
