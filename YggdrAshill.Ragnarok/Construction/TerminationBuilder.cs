using YggdrAshill.Ragnarok.Periodization;
using System.Linq;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal static class TerminationBuilder
    {
        internal static ITerminationBuilder Default { get; } = new None();
        private sealed class None :
            ITerminationBuilder,
            ITermination
        {
            public ITermination Build()
            {
                return this;
            }

            public ITerminationBuilder Configure(ITermination termination)
            {
                return new One(termination);
            }

            public void Terminate()
            {

            }
        }
        private sealed class One :
           ITerminationBuilder
        {
            private readonly ITermination first;

            internal One(ITermination first)
            {
                this.first = first;
            }

            public ITermination Build()
            {
                return first;
            }

            public ITerminationBuilder Configure(ITermination termination)
            {
                if (first == termination)
                {
                    return this;
                }

                return new Listed(new[] { first, termination });
            }
        }
        private sealed class Listed :
            ITerminationBuilder
        {
            private readonly IEnumerable<ITermination> terminations;

            internal Listed(IEnumerable<ITermination> terminations)
            {
                this.terminations = terminations;
            }

            public ITermination Build()
            {
                return new Termination(terminations.ToArray());
            }

            public ITerminationBuilder Configure(ITermination termination)
            {
                if (terminations.Contains(termination))
                {
                    return this;
                }

                return new Listed(terminations.Append(termination));
            }
        }
        private sealed class Termination :
            ITermination
        {
            private readonly ITermination[] terminations;

            internal Termination(ITermination[] terminations)
            {
                this.terminations = terminations;
            }

            public void Terminate()
            {
                foreach (var termination in terminations)
                {
                    termination.Terminate();
                }
            }
        }
    }
}
