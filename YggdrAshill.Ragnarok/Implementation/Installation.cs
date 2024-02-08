using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Installation : IInstallation
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

        public Installation(Action<IObjectContainer> onInstalled)
        {
            this.onInstalled = onInstalled;
        }

        public void Install(IObjectContainer container)
        {
            onInstalled.Invoke(container);
        }
    }
}
