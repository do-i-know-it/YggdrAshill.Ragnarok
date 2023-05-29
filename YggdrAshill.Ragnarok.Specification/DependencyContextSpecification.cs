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
            using var scope = new DependencyContext(solver).Build();

            var resolver = scope.Resolver.Resolve<IResolver>();

            Assert.That(resolver, Is.EqualTo(scope.Resolver));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateTemporalObjectPerRequest(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyClass>(Lifetime.Temporal);

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateLocalObjectPerLocalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyClass>(Lifetime.Local);

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateGlobalObjectPerGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyClass>(Lifetime.Global);

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyClass>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyClass>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveDependenciesFromParentScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<DualInterface1>(Lifetime.Global).AsImplementedInterfaces();

            using var parentScope = parentContext.Build();

            var childContext = parentScope.CreateContext();

            childContext.Register<DualInterface2>(Lifetime.Global).AsImplementedInterfaces();
            childContext.Register<IService, MultipleDependencyService>(Lifetime.Global);

            using var childScope = childContext.Build();

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
            var context = new DependencyContext(solver);

            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            context.Register<DependencyIntoInstance>(Lifetime.Global)
                .WithFieldsInjected()
                .From("fieldInjected", fieldInjected)
                .WithPropertiesInjected()
                .From("PropertyInjected", propertyInjected)
                .WithMethodInjected()
                .From("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var scope = context.Build();

            var resolved = scope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(resolved.FieldInjected, Is.EqualTo(fieldInjected));
            Assert.That(resolved.PropertyInjected, Is.EqualTo(propertyInjected));
            Assert.That(resolved.MethodInjected, Is.EqualTo(methodInjected));
        }

        [Test]
        public void ShouldResolveInstanceInstantiatedExternally()
        {
            var context = new DependencyContext();

            var instance = new NoDependencyService();

            context.RegisterInstance<IService>(instance);

            using var scope = context.Build();

            var resolved = scope.Resolver.Resolve<IService>();

            Assert.That(resolved, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveTemporalInstancePerRequest(ISolver solver)
        {
            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            var parentContext = new DependencyContext(solver);

            parentContext.Register(() => new DependencyIntoInstance(), Lifetime.Temporal)
                .WithFieldsInjected()
                .From("fieldInjected", fieldInjected)
                .WithPropertiesInjected()
                .From("PropertyInjected", propertyInjected)
                .WithMethodInjected()
                .From("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<DependencyIntoInstance>();
            var instance2 = parentScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

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

            var parentContext = new DependencyContext(solver);

            parentContext.Register(() => new DependencyIntoInstance(), Lifetime.Local)
                .WithFieldsInjected()
                .From("fieldInjected", fieldInjected)
                .WithPropertiesInjected()
                .From("PropertyInjected", propertyInjected)
                .WithMethodInjected()
                .From("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<DependencyIntoInstance>();
            var instance2 = parentScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

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

            var parentContext = new DependencyContext(solver);

            parentContext.Register(() => new DependencyIntoInstance(), Lifetime.Global)
                .WithFieldsInjected()
                .From("fieldInjected", fieldInjected)
                .WithPropertiesInjected()
                .From("PropertyInjected", propertyInjected)
                .WithMethodInjected()
                .From("methodInjected", methodInjected)
                .AsImplementedInterfaces()
                .AsSelf();

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<DependencyIntoInstance>();
            var instance2 = parentScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));
        }

        // TODO: Rename method.
        [Test]
        public void ShouldInstantiateRegisteredAsInterface()
        {
            var context = new DependencyContext();

            context.Register<MultipleInterfaceClass>(Lifetime.Global)
                .As<IInterfaceA>()
                .As<IInterfaceB>()
                .As<IInterfaceC>();

            using var scope = context.Build();

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

        // TODO: Rename method.
        [Test]
        public void ShouldInstantiateRegisteredAsImplementedInterfaces()
        {
            var context = new DependencyContext();

            context.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces();

            using var scope = context.Build();

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

        // TODO: Rename method.
        [Test]
        public void ShouldInstantiateRegisteredAsInterfaceAndSelf()
        {
            var context = new DependencyContext();

            context.Register<MultipleInterfaceClass>(Lifetime.Global)
                .As<IInterfaceA>()
                .As<IInterfaceB>()
                .As<IInterfaceC>()
                .AsSelf();

            using var scope = context.Build();

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

        // TODO: Rename method.
        [Test]
        public void ShouldInstantiateRegisteredAsImplementedInterfacesAndSelf()
        {
            var context = new DependencyContext();

            context.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces().AsSelf();

            using var scope = context.Build();

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

            parentContext.Register<DualInterface1>(Lifetime.Global).AsImplementedInterfaces();

            using var parentScope = parentContext.Build();

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

            using var childScope = childContext.Build();

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

            var parentContext = new DependencyContext(solver);

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

            using var parentScope = parentContext.Build();

            var parentPackage = parentScope.Resolver.Resolve<IServiceBundle<IService>>().Package;

            Assert.That(parentPackage.Count, Is.EqualTo(injectionCount + 1));
            foreach (var service in parentPackage)
            {
                Assert.That(service, Is.Not.Null);
            }

            var childContext = parentScope.CreateContext();

            childContext.Register<IService, NoDependencyService>(Lifetime.Global);

            using var childScope = childContext.Build();

            var childPackage = childScope.Resolver.Resolve<IServiceBundle<IService>>().Package;

            Assert.That(childPackage.Count, Is.EqualTo(injectionCount + 1));
            foreach (var service in childPackage)
            {
                Assert.That(service, Is.Not.Null);
            }
        }

        [Test]
        public void ShouldBuildScopeWithoutCircularDependency()
        {
            var parentContext = new DependencyContext();
            parentContext.Register<CircularDependencyClass1>(Lifetime.Temporal);

            using var parentScope = parentContext.Build();

            var childContext1 = parentScope.CreateContext();
            childContext1.Register<CircularDependencyClass2>(Lifetime.Temporal);

            var childContext2 = parentScope.CreateContext();
            childContext2.Register<CircularDependencyClass3>(Lifetime.Temporal);

            Assert.That(() =>
            {
                _ = childContext1.Build();

            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = childContext2.Build();

            }, Throws.Nothing);
        }

        [Test]
        public void ShouldDetectCircularDependencyInLocalScope()
        {
            var context = new DependencyContext();

            context.Register<CircularDependencyClass1>(Lifetime.Temporal);
            context.Register<CircularDependencyClass2>(Lifetime.Temporal);
            context.Register<CircularDependencyClass3>(Lifetime.Temporal);

            Assert.That(() =>
            {
                _ = context.Build();

            }, Throws.TypeOf<RagnarokCircularDependencyDetectedException>());

            Assert.That(() =>
            {
                _ = new DependencyContext().Build();

            }, Throws.Nothing);
        }

        // TODO: refactor to detect circular dependency in global scope.
        [Test]
        public void CannotDetectCircularDependencyInGlobalScope()
        {
            var parentContext = new DependencyContext();

            parentContext.Register<CircularDependencyClass1>(Lifetime.Temporal);

            using var parentScope = parentContext.Build();

            var childContext = parentScope.CreateContext();

            childContext.Register<CircularDependencyClass2>(Lifetime.Temporal);

            using var childScope = childContext.Build();

            var grandchildContext = childScope.CreateContext();

            grandchildContext.Register<CircularDependencyClass3>(Lifetime.Temporal);

            Assert.That(() =>
            {
                _ = grandchildContext.Build();

            }, Throws.Nothing);

            // StackOverflowException
            // grandchildContext.Build().Resolver.Resolve<CircularDependencyClass1>();
        }
    }
}
