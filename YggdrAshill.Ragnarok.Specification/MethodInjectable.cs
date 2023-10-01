namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class MethodInjectable
    {
        public NoDependencyClass Instance { get; private set; }

        [InjectMethod]
        private void Initialize(NoDependencyClass instance)
        {
            Instance = instance;
        }
    }
}
