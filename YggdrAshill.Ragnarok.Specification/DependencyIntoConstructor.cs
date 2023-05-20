namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class DependencyIntoConstructor
    {
        public NoDependencyClass NoDependencyClass { get; }

        [Inject]
        private DependencyIntoConstructor(NoDependencyClass noDependencyClass)
        {
            NoDependencyClass = noDependencyClass;
        }
    }
}
