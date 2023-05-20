namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class SingleDependencyService :
        IService
    {
        private readonly IInterfaceA interfaceA;

        [Inject]
        private SingleDependencyService(IInterfaceA interfaceA)
        {
            this.interfaceA = interfaceA;
        }
    }
}
