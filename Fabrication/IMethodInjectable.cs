namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IMethodInjectable :
        ITypeAssignment
    {
        IInjectIntoMethodExternally WithMethodInjected();
    }
}
