using YggdrAshill.Ragnarok.Progression;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// <see cref="ITermination"/> to execute each of connected <see cref="ITermination"/> simultaneously.
    /// </summary>
    public sealed class CompositeTermination :
        ITermination,
        IDisposable
    {
        private readonly List<ITermination> terminationList = new List<ITermination>();

        internal void Bind(ITermination termination)
        {
            if (terminationList.Contains(termination))
            {
                return;
            }

            terminationList.Add(termination);
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            foreach (var termination in terminationList)
            {
                termination.Terminate();
            }

            terminationList.Clear();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Terminate();
        }
    }
}
