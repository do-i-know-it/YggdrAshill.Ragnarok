namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IInvocation<in T>
        where T : notnull
    {
        void Invoke(T instance);
    }
}
