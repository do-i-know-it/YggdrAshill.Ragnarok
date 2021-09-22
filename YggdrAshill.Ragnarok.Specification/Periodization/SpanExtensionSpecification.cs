using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(SpanExtension))]
    internal class SpanExtensionSpecification
    {
        [Test]
        public void ShouldInitializeThenFinalize()
        {
            var origination = new FakeOrigination();
            var termination = new FakeTermination();

            using (origination.To(termination).Open())
            {
                Assert.IsTrue(origination.Originated);
            }

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotInitializeWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                using (default(ISpan).Open())
                {

                }
            });
        }
    }
}
