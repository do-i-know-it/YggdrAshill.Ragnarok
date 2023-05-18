using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class Installation :
        IInstallation
    {
        private readonly Action<IContainer> onInstalled;

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

        public Installation(Action<IContainer> onInstalled)
        {
            this.onInstalled = onInstalled;
        }

        public void Install(IContainer container)
        {
            onInstalled.Invoke(container);
        }
    }
}
