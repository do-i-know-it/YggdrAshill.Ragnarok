using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Origination))]
    internal class OriginationSpecification
    {
        [Test]
        public void ShouldExecuteFunctionWhenHasOriginated()
        {
            var expected = false;
            var origination = new Origination(() =>
            {
                expected = true;

                return new Termination();
            });

            var termination = origination.Originate();

            Assert.IsTrue(expected);

            termination.Terminate();
        }

        [Test]
        public void ShouldTerminateAfterHasOriginated()
        {
            var expected = false;
            var origination = new Origination(() =>
            {
                return new Termination(() =>
                {
                    expected = true;
                });
            });

            var termination = origination.Originate();

            termination.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new Origination(null);
            });
        }
    }
}
