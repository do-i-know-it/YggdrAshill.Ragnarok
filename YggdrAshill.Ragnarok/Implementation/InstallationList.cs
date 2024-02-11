using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments
    public sealed class InstallationList : IInstallation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Install(IObjectContainer container, IReadOnlyList<IInstallation> installationList)
        {
            if (installationList.Count == 0)
            {
                return;
            }

            foreach (var installation in installationList)
            {
                installation.Install(container);
            }
        }

        private readonly IInstallation[] installationList;

        public InstallationList(params IInstallation[] installationList)
        {
            this.installationList = installationList;
        }

        public void Install(IObjectContainer container)
        {
            Install(container, installationList);
        }
    }
}
