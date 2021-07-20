using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ExecutionExtension))]
    internal class ExecutionExtensionSpecification
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldExecuteIfConditionIsSatisfied(bool expected)
        {
            var execution = new FakeExecution();

            execution.When(new FakeCondition(expected)).Execute();

            Assert.AreEqual(expected, execution.Executed);
        }
    }
}
