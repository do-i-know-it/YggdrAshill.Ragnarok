namespace YggdrAshill.Ragnarok
{
    public interface IRootResolver : IObjectResolver
    {
        ISelector Selector { get; }
        ISolver Solver { get; }
    }
}
