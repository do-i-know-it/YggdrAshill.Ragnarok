using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Abortion))]
    internal class AbortionSpecification
    {
        [Test]
        public void ShouldExecuteActionWhenHasAborted()
        {
            var expected = false;
            var abortion = Abortion.Of(() =>
            {
                expected = true;
            });

            abortion.Abort(new Exception());

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
                abortion.Abort(null);
            });
        }
    }
}
