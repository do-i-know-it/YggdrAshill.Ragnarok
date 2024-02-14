namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IDependencyInjection : IInstanceInjection
    {
        IDependencyInjection WithArgument(IParameter parameter);
    }
}
