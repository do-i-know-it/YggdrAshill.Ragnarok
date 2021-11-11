using NUnit.Framework;
using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ServiceExtension))]
    internal class ServiceExtensionSpecification
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
        public void ShouldRun()
        {
            Service.Default
                .Configure(origination)
                .Configure(termination)
                .Configure(execution)
                .Run();

            Assert.IsTrue(origination.Originated);
            Assert.IsTrue(execution.Executed);
            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotRunWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IService).Run();
            });
        }
    }
}
