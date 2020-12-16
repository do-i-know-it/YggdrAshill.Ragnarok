using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Execution))]
    internal class ExecutionSpecification
    {
        [Test]
        public void ShouldExecuteActionWhenHasExecuted()
        {
            var expected = false;
            var execution = new Execution(() =>
            {
                expected = true;
            });

            execution.Execute();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = new Execution(null);
            });
        }
    }
}
