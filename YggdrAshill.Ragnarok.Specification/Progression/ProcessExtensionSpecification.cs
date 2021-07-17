using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ProcessExtension))]
    internal class ProcessExtensionSpecification :
        IProcess
    {
        private bool expected;
        public void Execute()
        {
            expected = true;
        }

        public void Originate()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        private IProcess process;

        [SetUp]
        public void SetUp()
        {
            expected = false;

            process = this;
        }

        [Test]
        public void ShouldConvertProcessIntoExecution()
        {
            process.Execution().Execute();

            Assert.IsTrue(expected);
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
