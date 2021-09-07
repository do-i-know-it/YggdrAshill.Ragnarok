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
    }
}
