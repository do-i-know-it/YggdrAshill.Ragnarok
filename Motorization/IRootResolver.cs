namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IRootResolver : IObjectResolver
    {
        ISelector Selector { get; }
        ISolver Solver { get; }
    }
}
