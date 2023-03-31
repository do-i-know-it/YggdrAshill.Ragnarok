namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class MethodInjectableClass
    {
        public InjectedClass InjectedClass { get; private set; }
        public InjectedStruct InjectedStruct { get; private set; }

        [InjectMethod]
        private void Initialize(InjectedClass injectedClass, InjectedStruct injectedStruct)
        {
            InjectedClass = injectedClass;
            InjectedStruct = injectedStruct;
        }
    }
}
