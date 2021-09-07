using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(PeriodExtension))]
    internal class PeriodExtensionSpecification
    {
        private FakePeriod period;

        private FakeExecution execution;

        [SetUp]
        public void SetUp()
        {
            period = new FakePeriod();

            execution = new FakeExecution();
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
        public void ShouldTransactExecution()
        {
            var transaction = period.Transact(execution);

            transaction.Execute();

            Assert.IsTrue(period.Originated);
            Assert.IsTrue(execution.Executed);
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
        public void CannotTransactWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var transaction = default(IPeriod).Transact(execution);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var transaction = period.Transact(default);
            });
        }
    }
}
