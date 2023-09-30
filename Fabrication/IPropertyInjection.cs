namespace YggdrAshill.Ragnarok
{
    public interface IPropertyInjection : IMethodInjectable
    {
        IPropertyInjection WithProperty(IParameter parameter);
    }
}
