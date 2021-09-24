using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(OriginationExtension))]
    internal class OriginationExtensionSpecification
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
        public void ShouldBeConvertedToSpanWithTermination()
        {
            var span = origination.To(termination);

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
                var span = default(IOrigination).To(termination);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = origination.To(default(ITermination));
            });
        }
    }
}
