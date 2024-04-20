namespace YggdrAshill.Ragnarok.Specification
{
    internal struct PropertyInjectable
    {
        [InjectProperty]
        public object Instance { get; private set; }
    }
}
