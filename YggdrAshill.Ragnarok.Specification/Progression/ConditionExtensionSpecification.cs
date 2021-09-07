using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ConditionExtension))]
    internal class ConditionExtensionSpecification
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBeInverted(bool expected)
        {
            var condition = new FakeCondition(expected);
            var inverted = condition.Not();

            Assert.AreEqual(!condition.IsSatisfied, inverted.IsSatisfied);
        }

        [TestCase(true, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        public void ShouldBeAdded(bool one, bool another, bool expected)
        {
            var added = new FakeCondition(one).Or(new FakeCondition(another));

            Assert.AreEqual(expected, added.IsSatisfied);
        }

        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void ShouldBeMultiplied(bool one, bool another, bool expected)
        {
            var multiplied = new FakeCondition(one).And(new FakeCondition(another));

            Assert.AreEqual(expected, multiplied.IsSatisfied);
        }

        [Test]
        public void CannotBeInvertedByNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var inverted = default(ICondition).Not();
            });
        }

        [Test]
        public void CannotBeAddedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var added = default(ICondition).Or(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var added = new FakeCondition(false).Or(default);
            });
        }

        [Test]
        public void CannotBeMultipliedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var multiplied = default(ICondition).And(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var multiplied = new FakeCondition(false).And(default);
            });
        }
    }
}
