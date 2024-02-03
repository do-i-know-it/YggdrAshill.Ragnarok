namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IRootResolverV2 : IObjectResolver
    {
        IDecision Decision { get; }
        IInstruction Instruction { get; }
    }
}
