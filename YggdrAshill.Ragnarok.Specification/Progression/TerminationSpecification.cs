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
            var origination = Termination.Of(() =>
            {
                expected = true;
            });

            origination.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = Termination.Of(null);
            });
        }
    }
}
