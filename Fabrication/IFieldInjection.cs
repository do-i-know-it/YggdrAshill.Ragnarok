namespace YggdrAshill.Ragnarok
{
    public interface IFieldInjection : IPropertyInjection
    {
        IPropertyInjection WithPropertyInjection();

        IFieldInjection WithField(IParameter parameter);
    }
}
