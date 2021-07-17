using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(TerminationExtension))]
    internal class TerminationExtensionSpecification :
        ITermination,
        IAbortion
    {
        private bool expected;
        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            expected = true;
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        private IAbortion abortion;

        private ITermination termination;

        [SetUp]
        public void SetUp()
        {
            expected = false;

            abortion = this;

            termination = this;
        }

        [Test]
        public void ShouldBindTermination()
        {
            termination.Bind(abortion).Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBindWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(ITermination).Bind(abortion);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                termination.Bind(default(IAbortion));
            });
        }
    }
}
