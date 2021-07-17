using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ProgressionExtension))]
    internal class ProgressionExtensionSpecification :
        IOrigination,
        ITermination,
        IExecution,
        IAbortion
    {
        private bool originated;
        public void Originate()
        {
            originated = true;
        }

        private bool terminated;
        public void Terminate()
        {
            terminated = true;
        }

        private bool executed;
        public void Execute()
        {
            executed = true;
        }

        private bool aborted;
        public void Abort(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            aborted = true;
        }

        private IOrigination origination;

        private ITermination termination;

        private IExecution execution;

        private IAbortion abortion;

        [SetUp]
        public void SetUp()
        {
            originated = false;

            terminated = false;

            executed = false;

            aborted = false;

            origination = this;

            termination = this;

            execution = this;

            abortion = this;
        }

        [Test]
        public void ShouldBindOrigination()
        {
            var composite = new CompositeOrigination();

            origination.Bind(composite);

            composite.Originate();

            Assert.IsTrue(originated);
        }

        [Test]
        public void ShouldBindTermination()
        {
            var composite = new CompositeTermination();

            termination.Bind(composite);

            composite.Terminate();

            Assert.IsTrue(terminated);
        }

        [Test]
        public void ShouldBindExecution()
        {
            var composite = new CompositeExecution();

            var termination = execution.Bind(composite);

            composite.Execute();

            Assert.IsTrue(executed);

            termination.Terminate();
        }

        [Test]
        public void ShouldBindAbortion()
        {
            var composite = new CompositeAbortion();

            abortion.Bind(composite);

            composite.Abort(new Exception());

            Assert.IsTrue(aborted);
        }

        [Test]
        public void CannotBindWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IOrigination).Bind(new CompositeOrigination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                origination.Bind(default(CompositeOrigination));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                default(ITermination).Bind(new CompositeTermination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                termination.Bind(default(CompositeTermination));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IExecution).Bind(new CompositeExecution());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                execution.Bind(default(CompositeExecution));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IAbortion).Bind(new CompositeAbortion());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                abortion.Bind(default);
            });
        }
    }
}
