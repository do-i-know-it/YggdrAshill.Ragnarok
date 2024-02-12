namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ObjectDependentDisposableInstallation : IInstallation
    {
        public object Instance { get; } = new object();

        public ObjectDependentDisposable? Disposable { get; private set; }

        public void Install(IObjectContainer container)
        {
            container.Register<ObjectDependentDisposable>(Lifetime.Temporal);
            container.RegisterInstance(Instance);
            container.RegisterCallback(resolver => Disposable = resolver.Resolve<ObjectDependentDisposable>());
        }
    }
}
