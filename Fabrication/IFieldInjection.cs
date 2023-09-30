namespace YggdrAshill.Ragnarok
{
    public interface IFieldInjection : IPropertyInjectable
    {
        IFieldInjection WithField(IParameter parameter);
    }
}
