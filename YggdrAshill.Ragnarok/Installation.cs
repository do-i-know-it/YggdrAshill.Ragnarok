using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok
{
    public sealed class Installation :
        IInstallation
    {
        private readonly Action<IContainer> onInstalled;

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
