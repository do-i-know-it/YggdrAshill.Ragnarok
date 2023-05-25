using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    // TODO: rename interface.
    public interface IInjectIntoMethodExternally :
        ITypeAssignment
    {
        IInjectIntoMethodExternally From(IParameter parameter);
    }
}
