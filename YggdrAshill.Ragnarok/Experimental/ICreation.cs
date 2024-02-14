namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ICreation<out T>
        where T : notnull
    {
        T Create();
    }
}
