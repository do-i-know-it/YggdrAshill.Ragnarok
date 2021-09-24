using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(TerminationExtension))]
    internal class TerminationExtensionSpecification
    {
        private FakeOrigination origination;

        private FakeTermination termination;

        [SetUp]
        public void SetUp()
        {
            origination = new FakeOrigination();

            termination = new FakeTermination();
        }


        [Test]
        public void ShouldBeConvertedToSpanWithOrigination()
        {
            var span = termination.From(origination);

            span.Originate();

            Assert.IsTrue(origination.Originated);

            span.Terminate();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotBeConvertedToSpanWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = default(ITermination).From(origination);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = termination.From(default(IOrigination));
            });
        }
    }
}
