using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoFieldsExternally :
        IPropertyInjectable
    {
        IInjectIntoFieldsExternally From(IParameter parameter);
    }
}
