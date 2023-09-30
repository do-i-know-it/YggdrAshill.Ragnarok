namespace YggdrAshill.Ragnarok
{
    public interface IMethodInjection : ITypeAssignment
    {
        IMethodInjection WithMethodArgument(IParameter parameter);
    }
}
