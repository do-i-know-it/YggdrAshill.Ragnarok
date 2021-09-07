using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Termination))]
    internal class TerminationSpecification
    {
        [Test]
        public void ShouldExecuteActionWhenHasTerminated()
        {
            var expected = false;
            var termination = Termination.Of(() =>
            {
                expected = true;
            });

            termination.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = Termination.Of(null);
            });
        }
    }
}
