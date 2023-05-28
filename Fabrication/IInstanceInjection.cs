namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename methods.
    public interface IInstanceInjection :
        IPropertyInjectable
    {
        IInjectIntoFieldsExternally WithFieldsInjected();
    }
}
