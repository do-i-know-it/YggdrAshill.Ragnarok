using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(TerminationExtension))]
    internal class TerminationExtensionSpecification
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBeBoundToCondition(bool expected)
        {
            var termination = new FakeTermination();

            termination.When(new FakeCondition(expected)).Terminate();

            Assert.AreEqual(expected, termination.Terminated);
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
            var termination = new ErroredTermination(expected);
            var abortion = new FakeAbortion();

            termination.Bind(abortion).Terminate();

            Assert.AreEqual(termination.Expected, abortion.Aborted);
        }

        [Test]
        public void CannotBeBoundWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = default(ITermination).When(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = new FakeTermination().When(default);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = default(ITermination).Bind(new FakeAbortion());
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = new FakeTermination().Bind(default(IAbortion));
            });
        }
    }
}
