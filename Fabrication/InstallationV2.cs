using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IInstallationV2"/>.
    /// </summary>
    public sealed class InstallationV2 : IInstallationV2
    {
        private readonly Action<IObjectContainer> onInstalled;

        public InstallationV2(IEnumerable<IInstallationV2> installationList)
        {
            onInstalled = container =>
            {
                foreach (var installation in installationList)
                {
                    installation.Install(container);
                }
            };
        }

        /// <summary>
        /// Creates <see cref="InstallationV2"/>.
        /// </summary>
        /// <param name="onInstalled">
        /// <see cref="Action{T}"/> to install <see cref="IObjectContainer"/>.
        /// </param>
        public InstallationV2(Action<IObjectContainer> onInstalled)
        {
            this.onInstalled = onInstalled;
        }

        /// <inheritdoc/>
        public void Install(IObjectContainer container)
        {
            onInstalled.Invoke(container);
        }
    }
}
