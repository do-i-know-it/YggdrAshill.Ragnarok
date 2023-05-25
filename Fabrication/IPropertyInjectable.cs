namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IPropertyInjectable :
        IMethodInjectable
    {
        IInjectIntoPropertiesExternally WithPropertiesInjected();
    }
}
