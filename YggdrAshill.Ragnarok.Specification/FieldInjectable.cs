namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class FieldInjectable
    {
        [InjectField]
        private object instance;
        public object Instance => instance;
    }
}
