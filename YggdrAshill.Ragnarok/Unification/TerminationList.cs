using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Unification;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    public sealed class TerminationList :
        ITerminationList
    {
        private readonly List<ITermination> terminationList = new List<ITermination>();

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
