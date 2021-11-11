using YggdrAshill.Ragnarok.Periodization;
using System.Linq;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal static class OriginationBuilder
    {
        internal static IOriginationBuilder Default { get; } = new None();
        private sealed class None :
            IOriginationBuilder,
            IOrigination
        {
            public IOrigination Build()
            {
                return this;
            }

            public IOriginationBuilder Configure(IOrigination origination)
            {
                return new One(origination);
            }

            public void Originate()
            {

            }
        }
        private sealed class One :
            IOriginationBuilder
        {
            private readonly IOrigination first;

            internal One(IOrigination first)
            {
                this.first = first;
            }

            public IOrigination Build()
            {
                return first;
            }

            public IOriginationBuilder Configure(IOrigination origination)
            {
                if (first == origination)
                {
                    return this;
                }

                return new Listed(new[] { first, origination });
            }
        }
        private sealed class Listed :
            IOriginationBuilder
        {
            private readonly IEnumerable<IOrigination> originations;

            internal Listed(IEnumerable<IOrigination> originations)
            {
                this.originations = originations;
            }

            public IOrigination Build()
            {
                return new Origination(originations.ToArray());
            }

            public IOriginationBuilder Configure(IOrigination origination)
            {
                if (originations.Contains(origination))
                {
                    return this;
                }

                return new Listed(originations.Append(origination));
            }
        }
        private sealed class Origination :
            IOrigination
        {
            private readonly IOrigination[] originations;

            internal Origination(IOrigination[] originations)
            {
                this.originations = originations;
            }

            public void Originate()
            {
                foreach (var origination in originations)
                {
                    origination.Originate();
                }
            }
        }
    }
}
