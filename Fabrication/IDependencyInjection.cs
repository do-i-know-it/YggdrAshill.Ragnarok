namespace YggdrAshill.Ragnarok
{
    public interface IDependencyInjection : IFieldInjection
    {
        IDependencyInjection WithArgument(IParameter parameter);
    }
}
