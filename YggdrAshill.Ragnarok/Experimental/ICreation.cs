namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    internal interface ICreation<out T>
        where T : notnull
    {
        T Create();
    }
}
