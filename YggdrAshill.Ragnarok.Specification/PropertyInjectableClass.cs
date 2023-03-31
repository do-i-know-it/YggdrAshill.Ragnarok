namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class PropertyInjectableClass
    {
        [InjectProperty] public InjectedClass InjectedClass { get; private set; }
        [InjectProperty] public InjectedStruct InjectedStruct { get; private set; }
    }
}
