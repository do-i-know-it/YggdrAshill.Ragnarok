using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(TerminationList))]
    internal class TerminationListSpecification
    {
        private TerminationList terminationList;

        [SetUp]
        public void SetUp()
        {
            terminationList = new TerminationList();
        }

        [TearDown]
        public void TearDown()
        {
            terminationList = default;
        }

        [Test]
        public void CollectedShouldTerminateWhenHasTerminated()
        {
            var expected = false;
            var termination = new Termination(() =>
            {
                expected = true;
            });

            terminationList.Collect(termination);

            terminationList.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotCollectNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                terminationList.Collect(null);
            });
        }
    }
}
