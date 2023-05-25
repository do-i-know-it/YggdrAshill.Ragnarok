using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IInjectIntoPropertiesExternally :
        IMethodInjectable
    {
        IInjectIntoPropertiesExternally From(IParameter parameter);
    }
}
