using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(OriginationExtension))]
    internal class OriginationExtensionSpecification :
        IOrigination,
        IAbortion
    {
        private bool expected;
        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            expected = true;
        }

        public void Originate()
        {
            throw new NotImplementedException();
        }

        private IAbortion abortion;

        private IOrigination origination;

        [SetUp]
        public void SetUp()
        {
            expected = false;

            abortion = this;

            origination = this;
        }

        [Test]
        public void ShouldBindOrigination()
        {
            origination.Bind(abortion).Originate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBindWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IOrigination).Bind(abortion);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                origination.Bind(default(IAbortion));
            });
        }
    }
}
