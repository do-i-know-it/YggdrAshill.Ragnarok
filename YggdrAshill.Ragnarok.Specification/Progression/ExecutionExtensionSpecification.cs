using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ExecutionExtension))]
    internal class ExecutionExtensionSpecification :
        IExecution,
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

        public void Execute()
        {
            throw new NotImplementedException();
        }

        private IAbortion abortion;

        private IExecution execution;

        [SetUp]
        public void SetUp()
        {
            expected = false;

            abortion = this;

            execution = this;
        }

        [Test]
        public void ShouldBindExecution()
        {
            execution.Bind(abortion).Execute();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBindWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IExecution).Bind(abortion);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                execution.Bind(default(IAbortion));
            });
        }
    }
}
