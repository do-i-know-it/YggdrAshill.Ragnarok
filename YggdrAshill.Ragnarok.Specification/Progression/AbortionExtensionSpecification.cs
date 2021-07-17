using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(AbortionExtension))]
    internal class AbortionExtensionSpecification :
        IOrigination,
        IExecution,
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

        public void Originate()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        private IAbortion abortion;

        private IOrigination origination;

        private IExecution execution;

        private ITermination termination;

        [SetUp]
        public void SetUp()
        {
            expected = false;

            abortion = this;

            origination = this;

            execution = this;

            termination = this;
        }

        [Test]
        public void ShouldBindOrigination()
        {
            origination.Bind(abortion).Originate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldBindExecution()
        {
            execution.Bind(abortion).Execute();

            Assert.IsTrue(expected);
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
                default(IOrigination).Bind(abortion);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                origination.Bind(default(IAbortion));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IExecution).Bind(abortion);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                execution.Bind(default(IAbortion));
            });

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
