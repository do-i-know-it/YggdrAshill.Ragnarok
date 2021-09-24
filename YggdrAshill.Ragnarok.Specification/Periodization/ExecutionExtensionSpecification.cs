using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ExecutionExtension))]
    internal class ExecutionExtensionSpecification
    {
        private FakeExecution execution;

        private FakeOrigination origination;

        private FakeTermination termination;

        [SetUp]
        public void SetUp()
        {
            execution = new FakeExecution();

            origination = new FakeOrigination();

            termination = new FakeTermination();
        }

        [Test]
        public void ShouldBeConvertedToCycleWithSpan()
        {
            var span = origination.To(termination);

            var cycle = execution.In(span);

            cycle.Originate();

            Assert.IsTrue(origination.Originated);

            cycle.Execute();

            Assert.IsTrue(execution.Executed);

            cycle.Terminate();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void ShouldBeConvertedToCycleWithOriginationAndTermination()
        {
            var cycle = execution.Between(origination, termination);

            cycle.Originate();

            Assert.IsTrue(origination.Originated);

            cycle.Execute();

            Assert.IsTrue(execution.Executed);

            cycle.Terminate();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotBeConvertedToCycleWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = default(IExecution).In(origination.To(termination));
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = execution.In(default(ISpan));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = default(IExecution).Between(origination, termination);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = execution.Between(default(IOrigination), termination);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var cycle = execution.Between(origination, default(ITermination));
            });
        }
    }
}
