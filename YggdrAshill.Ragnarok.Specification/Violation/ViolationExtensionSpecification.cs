using NUnit.Framework;
using YggdrAshill.Ragnarok.Violation;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ViolationExtension))]
    internal class ViolationExtensionSpecification :
        IAbortion
    {
        private bool hasAborted;

        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            hasAborted = true;
        }

        [SetUp]
        public void SetUp()
        {
            hasAborted = false;
        }

        [TearDown]
        public void TearDown()
        {
            hasAborted = false;
        }

        [Test]
        public void ShouldAbortWhenHasErroredInExecution()
        {
            var execution = new Execution(() => throw new Exception());

            var binded = execution.Bind(this);

            binded.Execute();

            Assert.IsTrue(hasAborted);
        }
    }
}
