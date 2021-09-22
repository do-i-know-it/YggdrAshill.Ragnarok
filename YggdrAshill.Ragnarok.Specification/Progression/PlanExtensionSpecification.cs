using NUnit.Framework;
using System;
using YggdrAshill.Ragnarok.Proceduralization;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(PlanExtension))]
    internal class PlanExtensionSpecification
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
        public void ShouldRun()
        {
            var plan = execution.Between(origination, termination);

            plan.Run();

            Assert.IsTrue(origination.Originated);

            Assert.IsTrue(execution.Executed);

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotRunWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IPlan).Run();
            });
        }
    }
}
