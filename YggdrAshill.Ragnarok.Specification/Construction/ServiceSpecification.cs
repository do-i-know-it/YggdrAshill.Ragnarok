using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Construction;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Service))]
    internal class ServiceSpecification
    {
        private FakeOrigination origination;

        private FakeTermination termination;

        private FakeExecution execution;

        private FakeSpan span;

        [SetUp]
        public void SetUp()
        {
            origination = new FakeOrigination();

            termination = new FakeTermination();

            execution = new FakeExecution();

            span = new FakeSpan();
        }

        [Test]
        public void CannotConfigureWithOrigination()
        {
            Service.Default.Configure(origination).Build().Run();

            Assert.IsTrue(origination.Originated);
        }

        [Test]
        public void CannotConfigureWithTermination()
        {
            Service.Default.Configure(termination).Build().Run();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotConfigureWithExecution()
        {
            Service.Default.Configure(execution).Build().Run();

            Assert.IsTrue(execution.Executed);
        }

        [Test]
        public void CannotConfigureWithSpan()
        {
            Service.Default.Configure(span).Build().Run();

            Assert.IsTrue(span.Originated);
            Assert.IsTrue(span.Terminated);
        }

        [Test]
        public void CannotConfigureWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.Configure(default(IOrigination));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.Configure(default(ITermination));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.Configure(default(IExecution));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.Configure(default(ISpan));
            });
        }
    }
}
