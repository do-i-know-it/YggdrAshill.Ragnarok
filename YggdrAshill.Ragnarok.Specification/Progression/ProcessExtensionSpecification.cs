using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ProcessExtension))]
    internal class ProcessExtensionSpecification
    {
        [Test]
        public void ShouldConvertProcessIntoExecution()
        {
            var process = new FakeProcess();

            process.Execution().Execute();

            Assert.IsTrue(process.Executed);
        }

        [Test]
        public void CannotConvertWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = default(IProcess).Execution();
            });
        }
    }
}
