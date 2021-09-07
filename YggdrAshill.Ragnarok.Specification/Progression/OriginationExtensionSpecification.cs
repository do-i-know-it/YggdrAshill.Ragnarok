using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(OriginationExtension))]
    internal class OriginationExtensionSpecification
    {
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
            var origination = new ErroredOrigination(expected);
            var abortion = new FakeAbortion();

            origination.Bind(abortion).Originate();

            Assert.AreEqual(origination.Expected, abortion.Aborted);
        }

        [Test]
        public void CannotBeBoundWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = default(IOrigination).When(new FakeCondition(false));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeOrigination().When(default(ICondition));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = default(IOrigination).Bind(new FakeAbortion());
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var origination = new FakeOrigination().Bind(default(IAbortion));
            });
        }
    }
}
