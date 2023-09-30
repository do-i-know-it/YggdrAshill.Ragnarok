namespace YggdrAshill.Ragnarok
{
    public interface IInstanceDependencyInjection : IPropertyDependencyInjection
    {
        IInstanceDependencyInjection WithField(IParameter parameter);
    }
}
