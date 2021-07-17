using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(PeriodExtension))]
    internal class PeriodExtensionSpecification :
        IPeriod
    {
        private bool originated;
        public void Originate()
        {
            originated = true;
        }

        private bool terminated;
        public void Terminate()
        {
            terminated = true;
        }

        private IPeriod period;

        [SetUp]
        public void SetUp()
        {
            originated = false;

            terminated = false;

            period = this;
        }

        [Test]
        public void ShouldConvertPeriodIntoOrigination()
        {
            period.Origination().Originate();

            Assert.IsTrue(originated);
        }

        [Test]
        public void ShouldConvertPeriodIntoTermination()
        {
            period.Termination().Terminate();

            Assert.IsTrue(terminated);
        }

        [Test]
        public void CannotConvertWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = default(IPeriod).Origination();
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = default(IPeriod).Termination();
            });
        }
    }
}
