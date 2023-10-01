namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class NoDependencyClassInstallation : IInstallation
    {
        public void Install(IObjectContainer container)
        {
            container.Register<NoDependencyClass>(Lifetime.Global);
        }
    }
}
