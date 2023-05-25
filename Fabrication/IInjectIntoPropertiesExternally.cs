using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoPropertiesExternally :
        IMethodInjectable
    {
        IInjectIntoPropertiesExternally From(IParameter parameter);
    }
}
