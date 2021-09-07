using NUnit.Framework;
using YggdrAshill.Ragnarok.Progression;
using System;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(ProgressionExtension))]
    internal class ProgressionExtensionSpecification
    {
        private static object[] TestSuiteForAbortion => new[]
        {
            new Exception(),
            new NotImplementedException(),
            new NotSupportedException(),
            new InvalidOperationException(),
        };

        private FakeOrigination origination;

        private FakeExecution  execution;

        private FakeTermination termination;

        private FakeAbortion abortion;

        [SetUp]
        public void SetUp()
        {
            origination = new FakeOrigination();

            execution = new FakeExecution();

            termination = new FakeTermination();

            abortion = new FakeAbortion();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ExecutionShouldBeBoundToCondition(bool expected)
        {
            execution.When(() => expected).Execute();

            Assert.AreEqual(expected, execution.Executed);
        }

        [TestCaseSource("TestSuiteForAbortion")]
        public void ExecutionShouldBeBoundToAbortion(Exception expected)
        {
            var aborted = default(Exception);
            new ErroredExecution(expected)
                .Bind(exception =>
                {
                    if (exception == null)
                    {
                        throw new ArgumentNullException(nameof(exception));
                    }

                    aborted = exception;
                })
                .Execute();

            Assert.AreEqual(expected, aborted);
        }

        [Test]
        public void OriginationShouldBeBoundToComposite()
        {
            var composite = new CompositeOrigination();

            origination.Bind(composite);

            composite.Originate();

            Assert.IsTrue(origination.Originated);
        }

        [Test]
        public void ExecutionShouldBeBoundToComposite()
        {
            var composite = new CompositeExecution();

            var termination = execution.Bind(composite);

            composite.Execute();

            Assert.IsTrue(execution.Executed);

            termination.Terminate();
        }

        [TestCaseSource("TestSuiteForAbortion")]
        public void AbortionShouldBeBoundToComposite(Exception expected)
        {
            var composite = new CompositeAbortion();

            abortion.Bind(composite);

            composite.Abort(expected);

            Assert.AreEqual(expected, abortion.Aborted);
        }

        [Test]
        public void TerminationShouldBeConvertedIntoDisposable()
        {
            termination.ToDisposable().Dispose();

            Assert.IsTrue(termination.Terminated);
        }

        [Test]
        public void CannotBeBoundToConditionWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = default(IExecution).When(() => false);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = Execution.None.When(default);
            });
        }

        [Test]
        public void CannotBeBoundToAbortionWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = default(IExecution).Bind(_ => { });
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var execution = Execution.None.Bind(default(Action<Exception>));
            });
        }

        [Test]
        public void CannotBeBoundToCompositeWithNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IOrigination).Bind(new CompositeOrigination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                Origination.None.Bind(default(CompositeOrigination));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = default(IExecution).Bind(new CompositeExecution());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var termination = Execution.None.Bind(default(CompositeExecution));
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                default(ITermination).Bind(new CompositeTermination());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                Termination.None.Bind(default(CompositeTermination));
            });
          
            Assert.Throws<ArgumentNullException>(() =>
            {
                default(IAbortion).Bind(new CompositeAbortion());
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                Abortion.None.Bind(default);
            });
        }
    }
}
