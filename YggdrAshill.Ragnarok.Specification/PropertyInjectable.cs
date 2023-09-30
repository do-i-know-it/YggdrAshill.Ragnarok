namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class PropertyInjectable
    {
        [InjectProperty]
        public NoDependencyClass Instance { get; private set; }
    }
}
