namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInstanceInjection :
        IPropertyInjectable
    {
        IInjectIntoFieldsExternally WithFieldsInjected();
    }
}
