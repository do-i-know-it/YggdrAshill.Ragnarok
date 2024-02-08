using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class Installation : IInstallation
    {
        private readonly Action<IObjectContainer> onInstalled;

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
