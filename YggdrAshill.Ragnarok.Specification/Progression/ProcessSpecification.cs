using NUnit.Framework;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(Process))]
    internal class ProcessSpecification
    {
        [Test]
        public void ShouldExecuteActionWhenHasOriginated()
        {
            var expected = false;
            var process = Process.Of(() =>
            {
                expected = true;
            }, () =>
            {

            }, () =>
            {

            });

            process.Originate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldExecuteActionWhenHasExecuted()
        {
            var expected = false;
            var process = Process.Of(() =>
            {

            }, () =>
            {
                expected = true;
            }, () =>
            {

            });

            process.Execute();

            Assert.IsTrue(expected);
        }

        [Test]
        public void ShouldExecuteActionWhenHasTerminated()
        {
            var expected = false;
            var process = Process.Of(() =>
            {

            }, () =>
            {

            }, () =>
            {
                expected = true;
            });

            process.Terminate();

            Assert.IsTrue(expected);
        }

        [Test]
        public void CannotBeGeneratedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var process = Process.Of(default, () => { }, () => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var process = Process.Of(() => { }, default, () => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var process = Process.Of(() => { }, () => { }, default);
            });
        }
    }
}
