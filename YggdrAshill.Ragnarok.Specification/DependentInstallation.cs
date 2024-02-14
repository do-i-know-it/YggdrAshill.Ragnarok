namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class DependentInstallation : IInstallation
    {
        private readonly int mock;

        [Inject]
        public DependentInstallation(int mock)
        {
            this.mock = mock;
        }

        public void Install(IObjectContainer container)
        {
            container.RegisterInstance(new object());
        }
    }
}
