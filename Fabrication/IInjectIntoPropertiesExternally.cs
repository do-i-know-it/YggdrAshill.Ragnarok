namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IInjectIntoPropertiesExternally :
        IMethodInjectable
    {
        IInjectIntoPropertiesExternally From(IParameter parameter);
    }
}
