using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoMethodExternally :
        ITypeAssignment
    {
        IInjectIntoMethodExternally From(IParameter parameter);
    }
}
