using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ConditionExtension))]
    internal class ConditionExtensionSpecification
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBeConverted(bool expected)
        {
            var condition = new FakeCondition(expected);
            Assert.AreEqual(!condition.IsSatisfied, condition.Not().IsSatisfied);
        }

        [TestCase(true, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        public void ShouldBeAdded(bool one, bool another, bool expected)
        {
            Assert.AreEqual(expected, new FakeCondition(one).Or(new FakeCondition(another)).IsSatisfied);
        }

        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void ShouldBeMultiplied(bool one, bool another, bool expected)
        {
            Assert.AreEqual(expected, new FakeCondition(one).And(new FakeCondition(another)).IsSatisfied);
        }
    }
}
