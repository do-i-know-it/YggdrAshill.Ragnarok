using NUnit.Framework;
using YggdrAshill.Ragnarok.Proceduralization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ProceduralizationExtension))]
    internal class ProceduralizationExtensionSpecification
    {
        private FakeExecution execution;

        [SetUp]
        public void SetUp()
        {
            execution = new FakeExecution();
        }

        [Test]
        public void ShouldBeConvertedToCycleWithAction()
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
