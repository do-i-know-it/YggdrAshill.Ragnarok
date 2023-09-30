namespace YggdrAshill.Ragnarok
{
    public interface IInstanceInjection : IFieldInjection
    {
        IFieldInjection WithFieldInjection();
    }
}
