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

        private FakeAbortion abortion;

        [SetUp]
        public void SetUp()
        {
            execution = new FakeExecution();

            period = new FakePeriod();

            abortion = new FakeAbortion();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBindToCondition(bool expected)
        {
            var bound = execution.If(new FakeCondition(expected));

            bound.Execute();

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
        public void CannotBindConditionWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = default(IExecution).If(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = execution.If(default(ICondition));
            });
        }

        [Test]
        public void CannotBindWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = default(IExecution).Bind(abortion);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var bound = execution.Bind(default(IAbortion));
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
