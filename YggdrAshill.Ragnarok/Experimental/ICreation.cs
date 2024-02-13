namespace YggdrAshill.Ragnarok
{
    public interface ICreation<out T>
        where T : notnull
    {
        T Create();
    }
}
