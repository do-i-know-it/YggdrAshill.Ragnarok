using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(PeriodizationExtension))]
    internal class PeriodizationExtensionSpecification
    {
        private FakeOrigination origination;

        private FakeTermination termination;

        private FakeExecution execution;

        [SetUp]
        public void SetUp()
        {
            origination = new FakeOrigination();

            termination = new FakeTermination();

            execution = new FakeExecution();
        }

        [Test]
        public void ShouldBeConvertedToSpanWithTerminationAsAction()
        {
            var expected = false;
            var span = origination.To(() =>
            {
                expected = true;
            });

            span.Originate();

            Assert.IsTrue(origination.Originated);

            span.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldBeConvertedToSpanWithOriginationAsAction()
        {
            var expected = false;
            var span = termination.From(() =>
            {
                expected = true;
            });

            span.Originate();

            Assert.IsTrue(expected);

            span.Terminate();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void ShouldBeConvertedToCycleWithActions()
        {
            var originated = false;
            var terminated = false;
            var cycle = execution.Between(() =>
            {
                originated = true;
            }, () =>
            {
                terminated = true;
            });

            cycle.Originate();

            Assert.IsTrue(originated);

            cycle.Execute();

            Assert.IsTrue(execution.Executed);

            cycle.Terminate();

            Assert.IsTrue(terminated);
        }

        [Test]
        public void CannotBeConvertedToSpanWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = default(IOrigination).To(() => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = origination.To(default(Action));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = default(ITermination).From(() => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = termination.From(default(Action));
            });
        }

        [Test]
        public void CannotBeConvertedToCycleWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = default(IExecution).Between(() => { }, () => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = execution.Between(default(Action), () => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = execution.Between(() => { }, default(Action));
            });
        }
    }
}
