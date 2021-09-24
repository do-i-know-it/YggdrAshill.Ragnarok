using NUnit.Framework;
using System;
using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(CycleExtension))]
    internal class CycleExtensionSpecification
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
            var cycle = execution.Between(origination, termination);

            cycle.Run();

            Assert.IsTrue(origination.Originated);

            Assert.IsTrue(execution.Executed);

            Assert.IsTrue(termination.Terminated);
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
