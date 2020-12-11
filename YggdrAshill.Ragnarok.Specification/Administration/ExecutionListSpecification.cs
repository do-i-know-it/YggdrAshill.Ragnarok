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

            executionList.Collect(execution);

            executionList.Execute();

            Assert.IsTrue(expected);
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
