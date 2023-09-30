namespace YggdrAshill.Ragnarok
{
    public interface IConstructorDependencyInjection : IInstanceDependencyInjection
    {
        IConstructorDependencyInjection WithArgument(IParameter parameter);
    }
}
