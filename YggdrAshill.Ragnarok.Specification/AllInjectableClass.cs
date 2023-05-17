namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class AllInjectableClass
    {
        public InjectedStruct ConstructorInjected { get; }

        [Inject]
        public AllInjectableClass(InjectedStruct constructorInjected)
        {
            ConstructorInjected = constructorInjected;
        }

        [InjectField] private InjectedStruct fieldInjected;
        public InjectedStruct FieldInjected => fieldInjected;

        [InjectProperty] public InjectedStruct PropertyInjected { get; private set; }

        public InjectedStruct MethodInjected { get; private set; }

        [InjectMethod]
        private void Initialize(InjectedStruct methodInjected)
        {
            MethodInjected = methodInjected;
        }
    }
}
