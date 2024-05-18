namespace YggdrAshill.Ragnarok.Specification
{
    internal struct MethodInjectable
    {
        public object Instance { get; private set; }

        [InjectMethod]
        private void Initialize(object instance)
        {
            Instance = instance;
        }
    }
}
