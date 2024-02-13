namespace YggdrAshill.Ragnarok
{
    public interface IInvocation<in T>
        where T : notnull
    {
        void Invoke(T instance);
    }
}
