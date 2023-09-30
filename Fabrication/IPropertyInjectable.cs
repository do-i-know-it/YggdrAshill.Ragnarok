namespace YggdrAshill.Ragnarok
{
    public interface IPropertyInjectable : IPropertyInjection
    {
        IPropertyInjection WithPropertyInjection();
    }
}
