namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ConstructorInjectable
    {
        public object Instance { get; }

        [Inject]
        public ConstructorInjectable(object instance)
        {
            Instance = instance;
        }
    }
}
