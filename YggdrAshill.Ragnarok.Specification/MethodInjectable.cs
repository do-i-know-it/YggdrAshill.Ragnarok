namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class MethodInjectable
    {
        public object Instance { get; private set; }

        [InjectMethod]
        private void Initialize(object instance)
        {
            Instance = instance;
        }
    }
}
