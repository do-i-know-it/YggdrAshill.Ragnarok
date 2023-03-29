namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ConstructorInjectableClass
    {
        public InjectedClass InjectedClass { get; }
        public InjectedStruct InjectedStruct { get; }

        [Inject]
        public ConstructorInjectableClass(InjectedClass injectedClass, InjectedStruct injectedStruct)
        {
            InjectedClass = injectedClass;
            InjectedStruct = injectedStruct;
        }
    }
}
