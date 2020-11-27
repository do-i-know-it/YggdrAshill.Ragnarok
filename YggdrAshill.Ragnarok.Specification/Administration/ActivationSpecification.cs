using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Activation))]
    internal class ActivationSpecification
    {
        [Test]
        public void ShouldExecuteFunctionWhenHasActivated()
        {
            var expected = false;
            var activation = new Activation(() =>
            {
                expected = true;

                return new Execution();
            });

            var execution = activation.Activate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldExecuteAfterHasActivated()
        {
            var expected = false;
            var activation = new Activation(() =>
            {
                return new Execution(() =>
                {
                    expected = true;
                });
            });

            var execution = activation.Activate();

            execution.Execute();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var observation = new Activation(null);
            });
        }
    }
}
