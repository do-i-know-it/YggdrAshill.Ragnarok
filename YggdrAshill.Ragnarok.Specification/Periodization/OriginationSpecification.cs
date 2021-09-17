using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Origination))]
    internal class OriginationSpecification
    {
        [Test]
        public void ShouldExecuteActionWhenHasOriginated()
        {
            var expected = false;
            var origination = Origination.Of(() =>
            {
                expected = true;
            });

            origination.Originate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = Origination.Of(null);
            });
        }
    }
}
