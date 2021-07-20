using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Condition))]
    internal class ConditionSpecification
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldExecuteFunction(bool expected)
        {
            var condition = Condition.Of(() => expected);

            Assert.AreEqual(expected, condition.IsSatisfied);
        }

        [Test]
        public void ShouldBeAlwaysSatisfied()
        {
            Assert.IsTrue(Condition.Always.IsSatisfied);
        }

        [Test]
        public void ShouldBeNeverSatisfied()
        {
            Assert.IsFalse(Condition.Never.IsSatisfied);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var condition = Condition.Of(default(Func<bool>));
            });
        }
    }
}
