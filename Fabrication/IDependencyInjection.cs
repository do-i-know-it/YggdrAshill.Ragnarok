using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IDependencyInjection :
        IInstanceInjection
    {
        IDependencyInjection WithArgument(IParameter parameter);
    }
}
