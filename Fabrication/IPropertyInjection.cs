namespace YggdrAshill.Ragnarok
{
    public interface IPropertyInjection : IMethodInjection
    {
        IMethodInjection WithMethodInjection();

        IPropertyInjection WithProperty(IParameter parameter);
    }
}
