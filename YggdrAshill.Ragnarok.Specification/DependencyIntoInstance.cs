namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class DependencyIntoInstance :
        IService
    {
        [InjectField]
        private NoDependencyClass fieldInjected;
        public NoDependencyClass FieldInjected => fieldInjected;

        [InjectProperty]
        public NoDependencyClass PropertyInjected { get; private set; }

        public NoDependencyClass MethodInjected { get; private set; }

        [InjectMethod]
        private void Initialize(NoDependencyClass methodInjected)
        {
            MethodInjected = methodInjected;
        }
    }
}
