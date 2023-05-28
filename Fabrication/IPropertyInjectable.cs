namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IPropertyInjectable :
        IMethodInjectable
    {
        IInjectIntoPropertiesExternally WithPropertiesInjected();
    }
}
