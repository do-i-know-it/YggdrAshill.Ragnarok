using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    /// <summary>
    /// Implementation of <see cref="IConfiguration"/> using <see cref="Action"/>.
    /// </summary>
    public sealed class Configuration :
        IConfiguration
    {
        private readonly Action<IDependencyContainer> onConfigured;

        public Configuration(IEnumerable<IConfiguration> configurationList)
        {
            onConfigured = container =>
            {
                foreach (var installation in configurationList)
                {
                    installation.Configure(container);
                }
            };
        }

        public Configuration(Action<IDependencyContainer> onConfigured)
        {
            this.onConfigured = onConfigured;
        }

        public void Configure(IDependencyContainer container)
        {
            onConfigured.Invoke(container);
        }
    }
}
