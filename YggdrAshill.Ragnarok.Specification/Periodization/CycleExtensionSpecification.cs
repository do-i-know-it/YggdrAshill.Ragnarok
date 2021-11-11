using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(CycleExtension))]
    internal class CycleExtensionSpecification
    {
        private FakeOrigination origination;

        private FakeTermination termination;

        private FakeSpan span;

        private FakeCycle cycle;

        [SetUp]
        public void SetUp()
        {
            origination = new FakeOrigination();

            termination = new FakeTermination();

            span = new FakeSpan();

            cycle = new FakeCycle();
        }

        [Test]
        public void ShouldBindToSpan()
        {
            var bound = cycle.In(span);

            bound.Run();

            Assert.IsTrue(span.Originated);
            Assert.IsTrue(span.Terminated);
        }

        [Test]
        public void ShouldBindToOriginationAndTermination()
        {
            var bound = cycle.Between(origination, termination);

            bound.Run();

            Assert.IsTrue(origination.Originated);
            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void ShouldRun()
        {
            cycle.Run();

            Assert.IsTrue(cycle.Originated);
            Assert.IsTrue(cycle.Executed);
            Assert.IsTrue(cycle.Terminated);
        }

        [Test]
        public void CannotBindToSpanWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = default(ICycle).In(span);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = cycle.In(default);
            });
        }

        [Test]
        public void CannotBindToOriginationAndTerminationWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = default(ICycle).Between(origination, termination);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = cycle.Between(default, termination);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = cycle.Between(origination, default);
            });
        }

        [Test]
        public void CannotRunWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(ICycle).Run();
            });
        }
    }
}
