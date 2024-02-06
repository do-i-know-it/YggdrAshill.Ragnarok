namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class InstallationWithDependency : IInstallation
    {
        private readonly object mock;

        [Inject]
        public InstallationWithDependency(object mock)
        {
            this.mock = mock;
        }

        public void Install(IObjectContainer container)
        {
            container.Register<NoDependencyClass>(Lifetime.Global);
        }
    }
}
