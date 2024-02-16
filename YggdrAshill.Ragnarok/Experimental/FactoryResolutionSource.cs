using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class FactoryResolutionSource : IFactoryResolution
    {
        public IObjectResolver Resolver { get; }

        public FactoryResolutionSource(IObjectResolver resolver)
        {
            Resolver = resolver;
        }

        private readonly List<IInstallation> installationList = new();

        public IReadOnlyList<IInstallation> InstallationList => installationList;

        public IFactoryResolution With(IInstallation installation)
        {
            if (!installationList.Contains(installation))
            {
                installationList.Add(installation);
            }

            return this;
        }
    }
}
