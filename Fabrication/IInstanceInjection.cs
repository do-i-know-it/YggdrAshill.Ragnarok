namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public interface IInstanceInjection :
        IPropertyInjectable
    {
        IInjectIntoFieldsExternally WithFieldsInjected();
    }
}
