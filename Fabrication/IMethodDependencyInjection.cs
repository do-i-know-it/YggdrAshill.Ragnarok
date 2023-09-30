namespace YggdrAshill.Ragnarok
{
    public interface IMethodDependencyInjection : ITypeAssignment
    {
        IMethodDependencyInjection WithMethodArgument(IParameter parameter);
    }
}
