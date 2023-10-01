using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IInstallation"/>.
    /// </summary>
    public sealed class Installation : IInstallation
    {
        private readonly Action<IObjectContainer> onInstalled;

        public Installation(IEnumerable<IInstallation> installationList)
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
        /// Creates <see cref="Installation"/>.
        /// </summary>
        /// <param name="onInstalled">
        /// <see cref="Action{T}"/> to install <see cref="IObjectContainer"/>.
        /// </param>
        public Installation(Action<IObjectContainer> onInstalled)
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
