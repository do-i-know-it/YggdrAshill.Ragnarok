namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class MultipleDependencyService :
        IService
    {
        private readonly IInterfaceA interfaceA;
        private readonly IInterfaceB interfaceB;
        private readonly IInterfaceC interfaceC;
        private readonly IInterfaceD interfaceD;

        [Inject]
        private MultipleDependencyService(IInterfaceA interfaceA, IInterfaceB interfaceB, IInterfaceC interfaceC, IInterfaceD interfaceD)
        {
            this.interfaceA = interfaceA;
            this.interfaceB = interfaceB;
            this.interfaceC = interfaceC;
            this.interfaceD = interfaceD;
        }
    }
}
