namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ObjectDependentDisposableInstallation : IInstallation, IInvocation<ObjectDependentDisposable>
    {
        public object Instance { get; } = new object();

        public ObjectDependentDisposable? Disposable { get; private set; }

        public void Install(IObjectContainer container)
        {
            container.Register<ObjectDependentDisposable>(Lifetime.Temporal);
            container.RegisterInstance(Instance);
            container.RegisterCallback(this);
        }

        public void Invoke(ObjectDependentDisposable instance)
        {
            Disposable = instance;
        }
    }
}
