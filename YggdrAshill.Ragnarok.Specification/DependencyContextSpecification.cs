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
            using var scope = new DependencyContextV2(solver).CreateScope();

            var resolver = scope.Resolver.Resolve<IObjectResolver>();

            Assert.That(resolver, Is.EqualTo(scope.Resolver));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateTemporalObjectPerRequest(ISolver solver)
        {
            var parentContext = new DependencyContextV2(solver);

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
            var parentContext = new DependencyContextV2(solver);

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
            var parentContext = new DependencyContextV2(solver);

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
        public void ShouldResolveDependenciesFromParentScope(ISolver solver)
        {
            var parentContext = new DependencyContextV2(solver);

            parentContext.Register<DualInterface1>(Lifetime.Global).AsImplementedInterfaces();

            using var parentScope = parentContext.CreateScope();

            var childContext = parentScope.CreateContext();

            childContext.Register<DualInterface2>(Lifetime.Global).AsImplementedInterfaces();
            childContext.Register<IService, MultipleDependencyService>(Lifetime.Global);

            using var childScope = childContext.CreateScope();

            Assert.That(() =>
            {
                _ = childScope.Resolver.Resolve<IService>();
            }, Throws.Nothing);

            var package = childScope.Resolver.Resolve<IServiceBundle<IService>>().Package;

            Assert.That(package.Count, Is.EqualTo(1));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectDependenciesIntoInstanceAfterEnabled(ISolver solver)
        {
            var context = new DependencyContextV2(solver);

            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            context.Register<DependencyIntoInstance>(Lifetime.Global)
                .WithField("fieldInjected", fieldInjected)
                .WithProperty("PropertyInjected", propertyInjected)
                .WithMethodArgument("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(resolved.FieldInjected, Is.EqualTo(fieldInjected));
            Assert.That(resolved.PropertyInjected, Is.EqualTo(propertyInjected));
            Assert.That(resolved.MethodInjected, Is.EqualTo(methodInjected));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveInstanceInstantiatedExternally(ISolver solver)
        {
            var context = new DependencyContextV2(solver);

            var instance = new NoDependencyService();

            context.RegisterInstance<IService>(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<IService>();

            Assert.That(resolved, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveTemporalInstancePerRequest(ISolver solver)
        {
            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            var parentContext = new DependencyContextV2(solver);

            parentContext.Register(() => new DependencyIntoInstance(), Lifetime.Temporal)
                .WithField("fieldInjected", fieldInjected)
                .WithProperty("PropertyInjected", propertyInjected)
                .WithMethodArgument("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<DependencyIntoInstance>();
            var instance2 = parentScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveLocalInstancePerLocalScope(ISolver solver)
        {
            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            var parentContext = new DependencyContextV2(solver);

            parentContext.Register(() => new DependencyIntoInstance(), Lifetime.Local)
                .WithField("fieldInjected", fieldInjected)
                .WithProperty("PropertyInjected", propertyInjected)
                .WithMethodArgument("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<DependencyIntoInstance>();
            var instance2 = parentScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveGlobalInstancePerGlobalScope(ISolver solver)
        {
            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            var parentContext = new DependencyContextV2(solver);

            parentContext.Register(() => new DependencyIntoInstance(), Lifetime.Global)
                .WithField("fieldInjected", fieldInjected)
                .WithProperty("PropertyInjected", propertyInjected)
                .WithMethodArgument("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<DependencyIntoInstance>();
            var instance2 = parentScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateScope();

            var instance3 = childScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveAsAssignedInterface(ISolver solver)
        {
            var context = new DependencyContextV2(solver);

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
            var context = new DependencyContextV2(solver);

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
            var context = new DependencyContextV2(solver);

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
            var context = new DependencyContextV2(solver);

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

            var parentContext = new DependencyContextV2(solver);

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

            parentContext.Register<DualInterface1>(Lifetime.Global).AsImplementedInterfaces();

            using var parentScope = parentContext.CreateScope();

            var parentArray = parentScope.Resolver.Resolve<IService[]>();
            var parentReadOnlyList = parentScope.Resolver.Resolve<IReadOnlyList<IService>>();
            var parentReadOnlyCollection = parentScope.Resolver.Resolve<IReadOnlyCollection<IService>>();
            var parentEnumerable = parentScope.Resolver.Resolve<IEnumerable<IService>>();

            Assert.That(parentArray.Length, Is.EqualTo(parentInjectionCount));
            Assert.That(parentReadOnlyList.Count, Is.EqualTo(parentInjectionCount));
            Assert.That(parentReadOnlyCollection.Count, Is.EqualTo(parentInjectionCount));
            Assert.That(parentEnumerable.Count(), Is.EqualTo(parentInjectionCount));
            foreach (var service in parentArray)
            {
                Assert.That(service, Is.Not.Null);
            }

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

            childContext.Register<DualInterface2>(Lifetime.Global).AsImplementedInterfaces();
            childContext.Register<IService, MultipleDependencyService>(Lifetime.Global);

            using var childScope = childContext.CreateScope();

            var childArray = childScope.Resolver.Resolve<IService[]>();
            var childReadOnlyList = childScope.Resolver.Resolve<IReadOnlyList<IService>>();
            var childReadOnlyCollection = childScope.Resolver.Resolve<IReadOnlyCollection<IService>>();
            var childEnumerable = childScope.Resolver.Resolve<IEnumerable<IService>>();

            var totalInjectionAmount = parentInjectionCount + childInjectionCount + 1;
            Assert.That(childArray.Length, Is.EqualTo(totalInjectionAmount));
            Assert.That(childReadOnlyList.Count, Is.EqualTo(totalInjectionAmount));
            Assert.That(childReadOnlyCollection.Count, Is.EqualTo(totalInjectionAmount));
            Assert.That(childEnumerable.Count(), Is.EqualTo(totalInjectionAmount));
            foreach (var service in parentArray)
            {
                Assert.That(service, Is.Not.Null);
            }
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveServiceBundle(ISolver solver)
        {
            var injectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyContextV2(solver);

            for (var count = 0; count < injectionCount; count++)
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

            parentContext.Register<IService, MultipleDependencyService>(Lifetime.Global);
            parentContext.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces();

            using var parentScope = parentContext.CreateScope();

            var parentPackage = parentScope.Resolver.Resolve<IServiceBundle<IService>>().Package;

            Assert.That(parentPackage.Count, Is.EqualTo(injectionCount + 1));
            foreach (var service in parentPackage)
            {
                Assert.That(service, Is.Not.Null);
            }

            var childContext = parentScope.CreateContext();

            childContext.Register<IService, NoDependencyService>(Lifetime.Global);

            using var childScope = childContext.CreateScope();

            var childPackage = childScope.Resolver.Resolve<IServiceBundle<IService>>().Package;

            Assert.That(childPackage.Count, Is.EqualTo(injectionCount + 1));
            foreach (var service in childPackage)
            {
                Assert.That(service, Is.Not.Null);
            }
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldCreateScopeWithoutCircularDependency(ISolver solver)
        {
            var parentContext = new DependencyContextV2(solver);
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
            var context = new DependencyContextV2(solver);

            context.Register<CircularDependencyClass1>(Lifetime.Temporal);
            context.Register<CircularDependencyClass2>(Lifetime.Temporal);
            context.Register<CircularDependencyClass3>(Lifetime.Temporal);

            Assert.That(() =>
            {
                _ = context.CreateScope();

            }, Throws.TypeOf<RagnarokCircularDependencyDetectedException>());

            Assert.That(() =>
            {
                _ = new DependencyContextV2(solver).CreateScope();

            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldDetectCircularDependencyInGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContextV2(solver);

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

            }, Throws.TypeOf<RagnarokCircularDependencyDetectedException>());
        }
    }
}
