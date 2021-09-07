using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ExecutionExtension))]
    internal class ExecutionExtensionSpecification
    {
        private FakeExecution execution;

        private FakePeriod period;

        [SetUp]
        public void SetUp()
        {
            execution = new FakeExecution();

            period = new FakePeriod();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBeBoundToCondition(bool expected)
        {
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
            var errored = new ErroredExecution(expected);
            var abortion = new FakeAbortion();

            var bound = errored.Bind(abortion);

            bound.Execute();

            Assert.AreEqual(errored.Expected, abortion.Aborted);
        }

        [Test]
        public void ShouldTransactInPeriod()
        {
            var transaction = execution.In(period);

            transaction.Execute();

            Assert.IsTrue(period.Originated);
            Assert.IsTrue(execution.Executed);
            Assert.IsTrue(period.Terminated);
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

        [Test]
        public void CannotTransactWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var transaction = default(IExecution).In(period);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var transaction = execution.In(default);
            });
        }
    }
}
