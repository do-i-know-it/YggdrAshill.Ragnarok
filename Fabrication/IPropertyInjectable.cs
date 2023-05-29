namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename interface.
    // TODO: rename methods.
    public interface IPropertyInjectable :
        IMethodInjectable
    {
        IInjectIntoPropertiesExternally WithPropertiesInjected();
    }
}
