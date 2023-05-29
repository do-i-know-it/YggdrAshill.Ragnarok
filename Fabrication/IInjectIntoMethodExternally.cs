namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IInjectIntoMethodExternally :
        ITypeAssignment
    {
        IInjectIntoMethodExternally From(IParameter parameter);
    }
}
