namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ConstructorInjectable
    {
        public NoDependencyClass Instance { get; }

        [Inject]
        public ConstructorInjectable(NoDependencyClass instance)
        {
            Instance = instance;
        }
    }
}
