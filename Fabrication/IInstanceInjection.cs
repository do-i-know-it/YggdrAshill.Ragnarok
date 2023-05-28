namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IInstanceInjection :
        IPropertyInjectable
    {
        IInjectIntoFieldsExternally WithFieldsInjected();
    }
}
