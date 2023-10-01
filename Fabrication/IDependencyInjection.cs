namespace YggdrAshill.Ragnarok
{
    public interface IDependencyInjection : IInstanceInjection
    {
        IDependencyInjection WithArgument(IParameter parameter);
    }
}
