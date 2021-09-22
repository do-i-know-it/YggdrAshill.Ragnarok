using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Proceduralization;
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
        public void ShouldBeConvertedToPlanWithSpan()
        {
            var span = origination.To(termination);

            var plan = execution.In(span);

            using (plan.Open())
            {
                Assert.IsTrue(origination.Originated);

                plan.Execute();

                Assert.IsTrue(execution.Executed);
            }

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void ShouldBeConvertedToPlanWithOriginationAndTermination()
        {
            var plan = execution.Between(origination, termination);

            using (plan.Open())
            {
                Assert.IsTrue(origination.Originated);

                plan.Execute();

                Assert.IsTrue(execution.Executed);
            }

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotBeConvertedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = default(IExecution).In(origination.To(termination));
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = execution.In(default(ISpan));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = default(IExecution).Between(origination, termination);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = execution.Between(default(IOrigination), termination);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = execution.Between(origination, default(ITermination));
            });
        }
    }
}
