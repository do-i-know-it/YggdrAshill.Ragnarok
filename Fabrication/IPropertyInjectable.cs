namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IPropertyInjectable :
        IMethodInjectable
    {
        IInjectIntoPropertiesExternally WithPropertiesInjected();
    }
}
