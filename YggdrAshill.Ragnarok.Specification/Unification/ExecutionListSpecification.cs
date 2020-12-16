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
        public void CollectedShouldExecuteWhenHasExecuted()
        {
            var expected = false;
            var execution = new Execution(() =>
            {
                expected = true;
            });

            var termination = executionList.Collect(execution);

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

            var termination = executionList.Collect(execution);

            termination.Terminate();

            executionList.Execute();

            Assert.IsFalse(expected);
        }

        [Test]
        public void CollectedShouldNotExecuteAfterHasTerminated()
        {
            var expected = false;
            var execution = new Execution(() =>
            {
                expected = true;
            });

            executionList.Collect(execution);

            executionList.Terminate();

            executionList.Execute();

            Assert.IsFalse(expected);
        }

        [Test]
        public void CannotCollectNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                executionList.Collect(null);
            });
        }
    }
}
