namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface ISubContainerResolution : ITypeAssignment
    {
        IObjectResolver Resolver { get; }

        ISubContainerResolution With(IInstallation installation);
    }
}
