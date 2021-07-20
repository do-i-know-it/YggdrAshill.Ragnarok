using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ConditionExtension))]
    internal class ConditionExtensionSpecification
    {
        [TestCase(true, true, false)]
        [TestCase(false, false, true)]
        public void ShouldCombineOrigination(bool condition, bool satisfied, bool unsatisfied)
        {
            var whenSatisfied = new FakeOrigination();
            var whenUnsatisfied = new FakeOrigination();

            new FakeCondition(condition).Combine(whenSatisfied, whenUnsatisfied).Originate();

            Assert.AreEqual(satisfied, whenSatisfied.Originated);
            Assert.AreEqual(unsatisfied, whenUnsatisfied.Originated);
        }

        [TestCase(true, true, false)]
        [TestCase(false, false, true)]
        public void ShouldCombineTermination(bool condition, bool satisfied, bool unsatisfied)
        {
            var whenSatisfied = new FakeTermination();
            var whenUnsatisfied = new FakeTermination();

            new FakeCondition(condition).Combine(whenSatisfied, whenUnsatisfied).Terminate();

            Assert.AreEqual(satisfied, whenSatisfied.Terminated);
            Assert.AreEqual(unsatisfied, whenUnsatisfied.Terminated);
        }

        [TestCase(true, true, false)]
        [TestCase(false, false, true)]
        public void ShouldCombineExecution(bool condition, bool satisfied, bool unsatisfied)
        {
            var whenSatisfied = new FakeExecution();
            var whenUnsatisfied = new FakeExecution();

            new FakeCondition(condition).Combine(whenSatisfied, whenUnsatisfied).Execute();

            Assert.AreEqual(satisfied, whenSatisfied.Executed);
            Assert.AreEqual(unsatisfied, whenUnsatisfied.Executed);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBeInverted(bool expected)
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

        [Test]
        public void CannotCombineWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = default(ICondition).Combine(new FakeOrigination(), new FakeOrigination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeCondition(false).Combine(default, new FakeOrigination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeCondition(false).Combine(new FakeOrigination(), default);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = default(ICondition).Combine(new FakeTermination(), new FakeTermination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeCondition(false).Combine(default, new FakeTermination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeCondition(false).Combine(new FakeTermination(), default);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = default(ICondition).Combine(new FakeExecution(), new FakeExecution());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeCondition(false).Combine(default, new FakeExecution());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeCondition(false).Combine(new FakeExecution(), default);
            });
        }

        [Test]
        public void CannotBeInvertedByNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var condition = default(ICondition).Not();
            });
        }

        [Test]
        public void CannotBeAddedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var condition = default(ICondition).Or(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var condition = new FakeCondition(false).Or(default);
            });
        }

        [Test]
        public void CannotBeMultipliedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var condition = default(ICondition).And(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var condition = new FakeCondition(false).And(default);
            });
        }
    }
}
