namespace YggdrAshill.Ragnarok
{
    public interface IPropertyDependencyInjection : IMethodDependencyInjection
    {
        IPropertyDependencyInjection WithProperty(IParameter parameter);
    }
}
