namespace YggdrAshill.Ragnarok
{
    public interface IConstructorInjection : IInstanceInjection
    {
        IConstructorInjection WithArgument(IParameter parameter);
    }
}
