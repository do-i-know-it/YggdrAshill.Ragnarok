namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IMethodInjectable :
        ITypeAssignment
    {
        IInjectIntoMethodExternally WithMethodInjected();
    }
}
