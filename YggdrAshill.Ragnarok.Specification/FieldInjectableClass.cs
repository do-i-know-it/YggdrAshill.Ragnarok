namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class FieldInjectableClass
    {
        [InjectField] private InjectedClass injectedClass;
        public InjectedClass InjectedClass => injectedClass;

        [InjectField] private InjectedStruct injectedStruct;
        public InjectedStruct InjectedStruct => injectedStruct;
    }
}
