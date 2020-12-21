using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ExecutionList))]
    internal class ExecutionListSpecification
    {
        private ExecutionList executionList;

        [SetUp]
        public void SetUp()
        {
            executionList = new ExecutionList();
        }

        [TearDown]
        public void TearDown()
        {
            executionList.Terminate();
            executionList = default;
        }

        [Test]
        public void BindedShouldExecuteWhenHasExecuted()
        {
            var expected = false;
            var execution = new Execution(() =>
            {
                expected = true;
            });

            var termination = executionList.Bind(execution);

            executionList.Execute();

            Assert.IsTrue(expected);

            termination.Terminate();
        }

        [Test]
        public void TerminatedShouldNotExecuteWhenHasExecuted()
        {
            var expected = false;
            var execution = new Execution(() =>
            {
                expected = true;
            });

            var termination = executionList.Bind(execution);

            termination.Terminate();

            executionList.Execute();

            Assert.IsFalse(expected);
        }

        [Test]
        public void BindedShouldNotExecuteAfterHasTerminated()
        {
            var expected = false;
            var execution = new Execution(() =>
            {
                expected = true;
            });

            executionList.Bind(execution);

            executionList.Terminate();

            executionList.Execute();

            Assert.IsFalse(expected);
        }

        [Test]
        public void CannotBindNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                executionList.Bind(null);
            });
        }
    }
}
