using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Abortion))]
    internal class AbortionSpecification
    {
        private static object[] TestSuiteForAbortion => new[]
        {
            new Exception(),
            new NotImplementedException(),
            new NotSupportedException(),
            new InvalidOperationException(),
        };
        [TestCaseSource("TestSuiteForAbortion")]
        public void ShouldExecuteActionWhenHasAborted(Exception exception)
        {
            var expected = false;
            var abortion = Abortion.Of(() =>
            {
                expected = true;
            });

            abortion.Abort(exception);

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var abortion = Abortion.Of(default(Action<Exception>));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var abortion = Abortion.Of(default(Action));
            });
        }

        [Test]
        public void CannotAbortWithNull()
        {
            var abortion = Abortion.None;

            Assert.Throws<ArgumentNullException>(() =>
            {
                abortion.Abort(default);
            });
        }
    }
}
