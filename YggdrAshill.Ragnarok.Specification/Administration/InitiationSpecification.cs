using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Initiation))]
    internal class InitiationSpecification
    {
        [Test]
        public void ShouldExecuteFunctionWhenHasInitiated()
        {
            var expected = false;
            var initiation = new Initiation(() =>
            {
                expected = true;

                return new Termination();
            });

            var termination = initiation.Initiate();

            Assert.IsTrue(expected);

            termination.Terminate();
        }

        [Test]
        public void ShouldTerminateAfterHasInitiated()
        {
            var expected = false;
            var initiation = new Initiation(() =>
            {
                return new Termination(() =>
                {
                    expected = true;
                });
            });

            var termination = initiation.Initiate();

            termination.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var observation = new Initiation(null);
            });
        }
    }
}
