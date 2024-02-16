namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IFactory<out T>
        where T : notnull
    {
        T Create();
    }

    // TODO: add document comments.
    public interface IFactory<in TInput, out TOutput>
        where TInput : notnull
        where TOutput : notnull
    {
        TOutput Create(TInput input);
    }
}
