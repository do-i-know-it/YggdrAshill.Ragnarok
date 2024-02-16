namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IFactoryResolution
    {
        IObjectResolver Resolver { get; }

        IFactoryResolution With(IInstallation installation);
    }
}
