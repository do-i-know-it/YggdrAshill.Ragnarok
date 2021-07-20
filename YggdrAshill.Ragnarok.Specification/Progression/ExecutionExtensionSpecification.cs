using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ExecutionExtension))]
    internal class ExecutionExtensionSpecification
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBeBoundToCondition(bool expected)
        {
            var execution = new FakeExecution();

            execution.When(new FakeCondition(expected)).Execute();

            Assert.AreEqual(expected, execution.Executed);
        }

        [Test]
        public void CannotBeBoundWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = default(IExecution).When(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = new FakeExecution().When(default);
            });
        }
    }
}
