using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Fabrication
{
    // TODO: add document comments.
    public interface IDependencyInjection :
        IInstanceInjection
    {
        IDependencyInjection WithArgument(IParameter parameter);
    }
}
