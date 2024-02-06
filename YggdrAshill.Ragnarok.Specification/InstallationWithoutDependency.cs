namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class InstallationWithoutDependency : IInstallation
    {
        public void Install(IObjectContainer container)
        {
            container.Register<NoDependencyClass>(Lifetime.Global);
        }
    }
}
