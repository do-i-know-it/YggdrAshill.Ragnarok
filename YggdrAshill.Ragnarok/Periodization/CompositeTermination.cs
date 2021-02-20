using YggdrAshill.Ragnarok.Periodization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="ITermination"/>.
    /// Collects other tokens of <see cref="ITermination"/> to execute when this has terminated.
    /// </summary>
    public sealed class CompositeTermination :
        ITermination
    {
        private readonly List<ITermination> terminationList = new List<ITermination>();

        /// <summary>
        /// Binds <see cref="ITermination"/>.
        /// </summary>
        /// <param name="termination">
        /// <see cref="ITermination"/> to bind.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="termination"/> is null.
        /// </exception>
        public void Bind(ITermination termination)
        {
            if (termination == null)
            {
                throw new ArgumentNullException(nameof(termination));
            }

            if (terminationList.Contains(termination))
            {
                return;
            }

            terminationList.Add(termination);
        }

        /// <summary>
        /// Executes each <see cref="ITermination"/> when this has terminated
        /// </summary>
        public void Terminate()
        {
            foreach (var termination in terminationList)
            {
                termination.Terminate();
            }

            terminationList.Clear();
        }
    }
}
