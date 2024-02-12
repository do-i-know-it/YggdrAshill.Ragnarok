namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class IndependentInstallation : IInstallation
    {
        public void Install(IObjectContainer container)
        {
            container.RegisterInstance(new object());
        }
    }
}
