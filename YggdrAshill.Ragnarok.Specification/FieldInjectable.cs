namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class FieldInjectable
    {
        [InjectField]
        private NoDependencyClass instance;
        public NoDependencyClass Instance => instance;
    }
}
