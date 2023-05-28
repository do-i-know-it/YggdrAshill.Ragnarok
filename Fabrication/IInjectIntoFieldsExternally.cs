namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IInjectIntoFieldsExternally :
        IPropertyInjectable
    {
        IInjectIntoFieldsExternally From(IParameter parameter);
    }
}
