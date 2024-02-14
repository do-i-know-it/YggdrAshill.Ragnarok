namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class PropertyInjectable
    {
        [InjectProperty]
        public object Instance { get; private set; }
    }
}
