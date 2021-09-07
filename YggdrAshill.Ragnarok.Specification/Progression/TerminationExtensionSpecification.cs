using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(TerminationExtension))]
    internal class TerminationExtensionSpecification
    {
        [Test]
        public void TerminationShouldBeConvertedIntoDisposable()
        {
            var termination = new FakeTermination();

            termination.ToDisposable().Dispose();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotBeConvertedIntoDisposableWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var disposable = default(ITermination).ToDisposable();
            });
        }
    }
}
