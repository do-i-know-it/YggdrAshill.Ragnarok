using YggdrAshill.Ragnarok.Progression;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Executes each of connected <see cref="IOrigination"/> simultaneously.
    /// </summary>
    public sealed class CompositeOrigination :
        IOrigination,
        IDisposable
    {
        private readonly List<IOrigination> originationList = new List<IOrigination>();

        /// <inheritdoc/>
        public void Originate()
        {
            foreach (var origination in originationList)
            {
                origination.Originate();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            originationList.Clear();
        }

        internal void Bind(IOrigination origination)
        {
            if (originationList.Contains(origination))
            {
                return;
            }

            originationList.Add(origination);
        }
    }
}
