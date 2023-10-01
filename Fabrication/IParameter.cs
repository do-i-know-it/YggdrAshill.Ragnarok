namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IParameter
    {
        bool CanResolve(Argument argument, out object instance);
    }
}
