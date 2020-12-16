using YggdrAshill.Ragnarok.Administration;
using System.Collections.Generic;
using System;
using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    public sealed class TerminationList :
        ITermination,
        ITerminationCollection
    {
        private readonly List<ITermination> terminationList = new List<ITermination>();

        public void Collect(ITermination termination)
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
