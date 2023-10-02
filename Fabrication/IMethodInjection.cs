namespace YggdrAshill.Ragnarok
{
    public interface IMethodInjection : ITypeAssignment
    {
        IMethodInjection WithMethod(IParameter parameter);
    }
}
