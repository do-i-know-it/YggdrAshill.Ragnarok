using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(PeriodExtension))]
    internal class PeriodExtensionSpecification
    {
        private FakePeriod period;

        [SetUp]
        public void SetUp()
        {
            period = new FakePeriod();
        }

        [Test]
        public void ShouldConvertPeriodIntoOrigination()
        {
            period.Origination().Originate();

            Assert.IsTrue(period.Originated);
        }

        [Test]
        public void ShouldConvertPeriodIntoTermination()
        {
            period.Termination().Terminate();

            Assert.IsTrue(period.Terminated);
        }

        [Test]
        public void ShouldInitializeThenFinalize()
        {
            using (period.Initialize())
            {
                Assert.IsTrue(period.Originated);
            }

            Assert.IsTrue(period.Terminated);
        }

        [Test]
        public void CannotInitializeWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var disposable = default(IPeriod).Initialize();
            });
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
