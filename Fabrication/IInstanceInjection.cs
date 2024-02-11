namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IInstanceInjection : IFieldInjection
    {
        IFieldInjection ResolvedImmediately();
    }
}
