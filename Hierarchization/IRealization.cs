namespace YggdrAshill.Ragnarok
{
    public interface IRealization
    {
        // TODO: object pooling.
        object[] Realize(IObjectResolver resolver);
    }
}
