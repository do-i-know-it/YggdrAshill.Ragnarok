using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
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
        public void ShouldBeConvertedToPlanWithAction()
        {
            var originated = false;
            var terminated = false;
            var plan = execution.In(() =>
            {
                originated = true;
            }, () =>
            {
                terminated = true;
            });

            using (plan.Scope())
            {
                Assert.IsTrue(originated);

                plan.Execute();

                Assert.IsTrue(execution.Executed);
            }

            Assert.IsTrue(terminated);
        }

        [Test]
        public void CannotBeConvertedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = default(IExecution).In(() => { }, () => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = execution.In(default(Action), () => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var plan = execution.In(() => { }, default(Action));
            });
        }
    }
}
