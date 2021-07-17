using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Period))]
    internal class PeriodSpecification
    {
        [Test]
        public void ShouldExecuteActionWhenHasOriginated()
        {
            var expected = false;
            var period = Period.Of(() =>
            {
                expected = true;
            }, () =>
            {

            });

            period.Originate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldExecuteActionWhenHasTerminated()
        {
            var expected = false;
            var period = Period.Of(() =>
            {

            }, () =>
            {
                expected = true;
            });

            period.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var period = Period.Of(default, () => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var period = Period.Of(() => { }, default);
            });
        }
    }
}
