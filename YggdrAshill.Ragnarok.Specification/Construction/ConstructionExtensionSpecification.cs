using NUnit.Framework;
using System;
using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ConstructionExtension))]
    internal class ConstructionExtensionSpecification
    {
        [Test]
        public void ShouldConfigureOriginationWithAction()
        {
            var expected = false;

            Service
                .Default
                .OnOriginated(() =>
                {
                    expected = true;
                })
                .Run();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldConfigureTerminationWithAction()
        {
            var expected = false;

            Service
                .Default
                .OnTerminated(() =>
                {
                    expected = true;
                })
                .Run();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldConfigureExecutionWithAction()
        {
            var expected = false;

            Service
                .Default
                .OnExecuted(() =>
                {
                    expected = true;
                })
                .Run();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldConfigureSpanWithActions()
        {
            var originated = false;
            var terminated = false;

            Service
                .Default
                .InSpan(() =>
                {
                    originated = true;
                }, ()=>
                {
                    terminated = true;
                })
                .Run();

            Assert.IsTrue(originated);
            Assert.IsTrue(terminated);
        }

        [Test]
        public void ConnotConfigureWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = default(IService).OnOriginated(() => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.OnOriginated(default);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = default(IService).OnTerminated(() => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.OnTerminated(default);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = default(IService).OnExecuted(() => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.OnExecuted(default);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = default(IService).InSpan(() => { }, () => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.InSpan(default, () => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var service = Service.Default.InSpan(() => { }, default);
            });
        }
    }
}
