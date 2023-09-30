using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(DependencyContext))]
    internal sealed class DependencyContextSpecification
    {
        private const int MinMultipleInjectionCount = 0;
        private const int MaxMultipleInjectionCount = 10;

        private static object[] SolverList { get; } =
        {
            ReflectionSolver.Instance,
            ExpressionSolver.Instance,
        };

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveResolver(ISolver solver)
        {
            using var scope = new DependencyContext(solver).CreateScope();

            var resolver = scope.Resolver.Resolve<IObjectResolver>();

            Assert.That(resolver, Is.EqualTo(scope.Resolver));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateTemporalObjectPerRequest(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyClass>(Lifetime.Temporal);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateLocalObjectPerLocalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyClass>(Lifetime.Local);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateGlobalObjectPerGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyClass>(Lifetime.Global);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveInstanceAlreadyCreated(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.RegisterInstance(instance);

            using var parentScope = context.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance));
            Assert.That(instance2, Is.EqualTo(instance));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance3, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveTemporalInstancePerRequest(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register(() => new NoDependencyClass(), Lifetime.Temporal);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveLocalInstancePerLocalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register(() => new NoDependencyClass(), Lifetime.Local);
            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveGlobalInstancePerGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register(() => new NoDependencyClass(), Lifetime.Global);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveAsAssignedInterface(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<MultipleInterfaceClass>(Lifetime.Global)
                .As<IInterfaceA>()
                .As<IInterfaceB>()
                .As<IInterfaceC>();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<MultipleInterfaceClass>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IInterfaceD>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            var interfaceA = scope.Resolver.Resolve<IInterfaceA>();
            var interfaceB = scope.Resolver.Resolve<IInterfaceB>();
            var interfaceC = scope.Resolver.Resolve<IInterfaceC>();

            Assert.That(interfaceA is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceB is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceC is MultipleInterfaceClass, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveAsImplementedInterfaces(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<MultipleInterfaceClass>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            var interfaceA = scope.Resolver.Resolve<IInterfaceA>();
            var interfaceB = scope.Resolver.Resolve<IInterfaceB>();
            var interfaceC = scope.Resolver.Resolve<IInterfaceC>();
            var interfaceD = scope.Resolver.Resolve<IInterfaceD>();

            Assert.That(interfaceA is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceB is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceC is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceD is MultipleInterfaceClass, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveAsAssignedInterfaceAndSelf(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<MultipleInterfaceClass>(Lifetime.Global)
                .As<IInterfaceA>()
                .As<IInterfaceB>()
                .As<IInterfaceC>()
                .AsSelf();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<MultipleInterfaceClass>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IInterfaceD>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            var interfaceA = scope.Resolver.Resolve<IInterfaceA>();
            var interfaceB = scope.Resolver.Resolve<IInterfaceB>();
            var interfaceC = scope.Resolver.Resolve<IInterfaceC>();

            Assert.That(interfaceA is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceB is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceC is MultipleInterfaceClass, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveAsImplementedInterfacesAndSelf(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces().AsSelf();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<MultipleInterfaceClass>();
            }, Throws.Nothing);

            var interfaceA = scope.Resolver.Resolve<IInterfaceA>();
            var interfaceB = scope.Resolver.Resolve<IInterfaceB>();
            var interfaceC = scope.Resolver.Resolve<IInterfaceC>();
            var interfaceD = scope.Resolver.Resolve<IInterfaceD>();

            Assert.That(interfaceA is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceB is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceC is MultipleInterfaceClass, Is.True);
            Assert.That(interfaceD is MultipleInterfaceClass, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveCollection(ISolver solver)
        {
            var parentInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyContext(solver);

            for (var count = 0; count < parentInjectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    parentContext.Register<IService, NoDependencyService>(Lifetime.Local);
                }
                else
                {
                    parentContext.Register<IService, NoDependencyService>(Lifetime.Temporal);
                }
            }

            parentContext.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces();
            parentContext.Register<IService, MultipleDependencyService>(Lifetime.Global);

            using var parentScope = parentContext.CreateScope();

            var totalParentInjectionAmount = parentInjectionCount + 1;

            var parentArray = parentScope.Resolver.Resolve<IService[]>();
            var parentReadOnlyList = parentScope.Resolver.Resolve<IReadOnlyList<IService>>();
            var parentReadOnlyCollection = parentScope.Resolver.Resolve<IReadOnlyCollection<IService>>();
            var parentEnumerable = parentScope.Resolver.Resolve<IEnumerable<IService>>();

            Assert.That(parentArray.Length, Is.EqualTo(totalParentInjectionAmount));
            Assert.That(parentReadOnlyList.Count, Is.EqualTo(totalParentInjectionAmount));
            Assert.That(parentReadOnlyCollection.Count, Is.EqualTo(totalParentInjectionAmount));
            Assert.That(parentEnumerable.Count(), Is.EqualTo(totalParentInjectionAmount));

            var childInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    childContext.Register<IService, NoDependencyService>(Lifetime.Local);
                }
                else
                {
                    childContext.Register<IService, NoDependencyService>(Lifetime.Temporal);
                }
            }

            using var childScope = childContext.CreateScope();

            var childArray = childScope.Resolver.Resolve<IService[]>();
            var childReadOnlyList = childScope.Resolver.Resolve<IReadOnlyList<IService>>();
            var childReadOnlyCollection = childScope.Resolver.Resolve<IReadOnlyCollection<IService>>();
            var childEnumerable = childScope.Resolver.Resolve<IEnumerable<IService>>();

            var totalChildInjectionAmount = parentInjectionCount + childInjectionCount + 1;
            Assert.That(childArray.Length, Is.EqualTo(totalChildInjectionAmount));
            Assert.That(childReadOnlyList.Count, Is.EqualTo(totalChildInjectionAmount));
            Assert.That(childReadOnlyCollection.Count, Is.EqualTo(totalChildInjectionAmount));
            Assert.That(childEnumerable.Count(), Is.EqualTo(totalChildInjectionAmount));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveServiceBundle(ISolver solver)
        {
            var parentInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyContext(solver);

            for (var count = 0; count < parentInjectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    parentContext.Register<IService, NoDependencyService>(Lifetime.Local);
                }
                else
                {
                    parentContext.Register<IService, NoDependencyService>(Lifetime.Temporal);
                }
            }

            parentContext.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces();
            parentContext.Register<IService, MultipleDependencyService>(Lifetime.Global);

            using var parentScope = parentContext.CreateScope();

            var parentPackage = parentScope.Resolver.Resolve<IServiceBundle<IService>>().Package;
            var totalParentInjectionAmount = parentInjectionCount + 1;
            Assert.That(parentPackage.Count, Is.EqualTo(totalParentInjectionAmount));

            var childInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    childContext.Register<IService, NoDependencyService>(Lifetime.Local);
                }
                else
                {
                    childContext.Register<IService, NoDependencyService>(Lifetime.Temporal);
                }
            }

            using var childScope = childContext.CreateScope();

            var childPackage = childScope.Resolver.Resolve<IServiceBundle<IService>>().Package;
            var totalChildInjectionAmount = parentInjectionCount + childInjectionCount;
            Assert.That(childPackage.Count, Is.EqualTo(totalChildInjectionAmount));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectDependencyIntoField(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyClass>(Lifetime.Global);
            context.Register<FieldInjectable>(Lifetime.Global).WithFieldInjection();

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<FieldInjectable>();

            Assert.That(resolved.Instance, Is.Not.Null);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectDependencyIntoProperty(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyClass>(Lifetime.Global);
            context.Register<PropertyInjectable>(Lifetime.Global).WithPropertyInjection();

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<PropertyInjectable>();

            Assert.That(resolved.Instance, Is.Not.Null);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectDependencyIntoMethod(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyClass>(Lifetime.Global);
            context.Register<MethodInjectable>(Lifetime.Global).WithMethodInjection();

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<MethodInjectable>();

            Assert.That(resolved.Instance, Is.Not.Null);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectInstance(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.Register<ConstructorInjectable>(Lifetime.Global).WithArgument("instance", instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<ConstructorInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectInstanceIntoField(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.Register<FieldInjectable>(Lifetime.Global).WithField("instance", instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<FieldInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectInstanceIntoProperty(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.Register<PropertyInjectable>(Lifetime.Global).WithProperty("Instance", instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<PropertyInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectInstanceIntoMethod(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.Register<MethodInjectable>(Lifetime.Global).WithMethodArgument("instance", instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<MethodInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldCreateScopeWithoutCircularDependency(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);
            parentContext.Register<CircularDependencyClass1>(Lifetime.Temporal);

            using var parentScope = parentContext.CreateScope();

            var childContext1 = parentScope.CreateContext();
            childContext1.Register<CircularDependencyClass2>(Lifetime.Temporal);

            var childContext2 = parentScope.CreateContext();
            childContext2.Register<CircularDependencyClass3>(Lifetime.Temporal);

            Assert.That(() =>
            {
                _ = childContext1.CreateScope();

            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = childContext2.CreateScope();

            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldDetectCircularDependencyInLocalScope(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<CircularDependencyClass1>(Lifetime.Temporal);
            context.Register<CircularDependencyClass2>(Lifetime.Temporal);
            context.Register<CircularDependencyClass3>(Lifetime.Temporal);

            Assert.That(() =>
            {
                _ = context.CreateScope();

            }, Throws.TypeOf<RagnarokCircularDependencyException>());

            Assert.That(() =>
            {
                _ = new DependencyContext(solver).CreateScope();

            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldDetectCircularDependencyInGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<CircularDependencyClass1>(Lifetime.Temporal);

            using var parentScope = parentContext.CreateScope();

            var childContext = parentScope.CreateContext();

            childContext.Register<CircularDependencyClass2>(Lifetime.Temporal);

            using var childScope = childContext.CreateScope();

            var grandchildContext = childScope.CreateContext();

            grandchildContext.Register<CircularDependencyClass3>(Lifetime.Temporal);

            Assert.That(() =>
            {
                _ = grandchildContext.CreateScope();

            }, Throws.TypeOf<RagnarokCircularDependencyException>());
        }
    }
}
