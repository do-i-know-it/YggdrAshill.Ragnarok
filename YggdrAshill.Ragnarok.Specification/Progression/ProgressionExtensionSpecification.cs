using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ProgressionExtension))]
    internal class ProgressionExtensionSpecification
    {
        private IProcess process;

        private IAbortion abortion;

        private bool originated;
        
        private bool executed;
        
        private bool terminated;

        private bool aborted;

        [SetUp]
        public void SetUp()
        {
            originated = false;

            executed = false;

            terminated = false;

            process = Process.Of(() =>
            {
                originated = true;
            }, () =>
            {
                executed = true;
            }, () =>
            {
                terminated = true;
            });

            aborted = false;

            abortion = Abortion.Of(exception =>
            {
                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                aborted = true;
            });
        }

        [Test]
        public void ShouldBindOrigination()
        {
            var composite = new CompositeOrigination();

            process.Origination().Bind(composite);

            composite.Originate();

            Assert.IsTrue(originated);
        }

        [Test]
        public void ShouldBindExecution()
        {
            var composite = new CompositeExecution();

            var termination = process.Execution().Bind(composite);

            composite.Execute();

            Assert.IsTrue(executed);

            termination.Terminate();
        }

        [Test]
        public void ShouldBindTermination()
        {
            var composite = new CompositeTermination();

            process.Termination().Bind(composite);

            composite.Terminate();

            Assert.IsTrue(terminated);
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
                process.Origination().Bind(default(CompositeOrigination));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IExecution).Bind(new CompositeExecution());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                process.Execution().Bind(default(CompositeExecution));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                default(ITermination).Bind(new CompositeTermination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                process.Termination().Bind(default(CompositeTermination));
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
