using NUnit.Framework;
using YggdrAshill.Ragnarok.Periodization;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(PeriodizationExtension))]
    internal class PeriodizationExtensionSpecification
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
            var expected = false;
            var span = origination.To(() =>
            {
                expected = true;
            });

            span.Originate();

            Assert.IsTrue(origination.Originated);

            span.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldBeConvertedToSpanWithOrigination()
        {
            var expected = false;
            var span = termination.From(() =>
            {
                expected = true;
            });

            span.Originate();

            Assert.IsTrue(expected);

            span.Terminate();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotBeConvertedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = default(IOrigination).To(() => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = origination.To(default(Action));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = default(ITermination).From(() => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var span = termination.From(default(Action));
            });
        }
    }
}
