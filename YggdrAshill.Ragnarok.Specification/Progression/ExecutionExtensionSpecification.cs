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

        private static object[] TestSuiteForAbortion => new[]
        {
            new Exception(),
            new NotImplementedException(),
            new NotSupportedException(),
            new InvalidOperationException(),
        };
        [TestCaseSource("TestSuiteForAbortion")]
        public void ShouldBeBoundToAbortion(Exception expected)
        {
            var execution = new ErroredExection(expected);
            var abortion = new FakeAbortion();

            execution.Bind(abortion).Execute();

            Assert.AreEqual(execution.Expected, abortion.Aborted);
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

            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = default(IExecution).Bind(new FakeAbortion());
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = new FakeExecution().Bind(default(IAbortion));
            });
        }
    }
}