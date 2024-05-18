namespace YggdrAshill.Ragnarok.Specification
{
    internal struct FieldInjectable
    {
        [InjectField]
        private object instance;
        public object Instance => instance;
    }
}
