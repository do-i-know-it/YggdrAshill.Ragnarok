using NUnit.Framework;
using YggdrAshill.Ragnarok.Construction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(DependencyInjectionContext))]
    internal sealed class DependencyInjectionContextSpecification
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
            using var scope = new DependencyInjectionContext(solver).Build();

            var resolver = scope.Resolver.Resolve<IResolver>();

            Assert.That(resolver, Is.EqualTo(scope.Resolver));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateTemporalObjectPerRequest(ISolver solver)
        {
            var parentContext = new DependencyInjectionContext(solver);

            parentContext.RegisterTemporal<NoDependencyClass>();

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
            var parentContext = new DependencyInjectionContext(solver);

            parentContext.RegisterLocal<NoDependencyClass>();

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
            var parentContext = new DependencyInjectionContext(solver);

            parentContext.RegisterGlobal<NoDependencyClass>();

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
            var parentContext = new DependencyInjectionContext(solver);

            parentContext.RegisterGlobal<DualInterface1>().AsImplementedInterfaces();

            using var parentScope = parentContext.Build();

            var childContext = parentScope.CreateContext();

            childContext.RegisterGlobal<DualInterface2>().AsImplementedInterfaces();
            childContext.RegisterGlobal<IService, MultipleDependencyService>();

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
            var context = new DependencyInjectionContext(solver);

            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            context.RegisterGlobal<DependencyIntoInstance>()
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
            var context = new DependencyInjectionContext();

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

            var parentContext = new DependencyInjectionContext(solver);

            parentContext.RegisterTemporalInstance(() => new DependencyIntoInstance())
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

            var parentContext = new DependencyInjectionContext(solver);

            parentContext.RegisterLocalInstance(() => new DependencyIntoInstance())
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

            var parentContext = new DependencyInjectionContext(solver);

            parentContext.RegisterGlobalInstance(() => new DependencyIntoInstance())
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
            var context = new DependencyInjectionContext();

            context.RegisterGlobal<MultipleInterfaceClass>()
                .As<IInterfaceA>()
                .As<IInterfaceB>()
                .As<IInterfaceC>();

            using var scope = context.Build();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<MultipleInterfaceClass>();
            }, Throws.TypeOf<Exception>());

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IInterfaceD>();
            }, Throws.TypeOf<Exception>());

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
            var context = new DependencyInjectionContext();

            context.RegisterGlobal<MultipleInterfaceClass>().AsImplementedInterfaces();

            using var scope = context.Build();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<MultipleInterfaceClass>();
            }, Throws.TypeOf<Exception>());

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
            var context = new DependencyInjectionContext();

            context.RegisterGlobal<MultipleInterfaceClass>()
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
            }, Throws.TypeOf<Exception>());

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
            var context = new DependencyInjectionContext();

            context.RegisterGlobal<MultipleInterfaceClass>().AsImplementedInterfaces().AsSelf();

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

            var parentContext = new DependencyInjectionContext(solver);

            for (var count = 0; count < parentInjectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    parentContext.RegisterLocal<IService, NoDependencyService>();
                }
                else
                {
                    parentContext.RegisterTemporal<IService, NoDependencyService>();
                }
            }

            parentContext.RegisterGlobal<DualInterface1>().AsImplementedInterfaces();

            using var parentScope = parentContext.Build();

            var parentArray = parentScope.Resolver.Resolve<IService[]>();
            var parentReadOnlyList = parentScope.Resolver.Resolve<IReadOnlyList<IService>>();
            var parentReadOnlyCollection = parentScope.Resolver.Resolve<IReadOnlyCollection<IService>>();
            var parentEnumerable = parentScope.Resolver.Resolve<IEnumerable<IService>>();

            Assert.That(parentArray.Length, Is.EqualTo(parentInjectionCount));
            Assert.That(parentReadOnlyList.Count, Is.EqualTo(parentInjectionCount));
            Assert.That(parentReadOnlyCollection.Count, Is.EqualTo(parentInjectionCount));
            Assert.That(parentEnumerable.Count(), Is.EqualTo(parentInjectionCount));

            var childInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    childContext.RegisterLocal<IService, NoDependencyService>();
                }
                else
                {
                    childContext.RegisterTemporal<IService, NoDependencyService>();
                }
            }

            childContext.RegisterGlobal<DualInterface2>().AsImplementedInterfaces();
            childContext.RegisterGlobal<IService, MultipleDependencyService>();

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
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveServiceBundle(ISolver solver)
        {
            var injectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext(solver);

            for (var count = 0; count < injectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    parentContext.RegisterLocal<IService, NoDependencyService>();
                }
                else
                {
                    parentContext.RegisterTemporal<IService, NoDependencyService>();
                }
            }

            parentContext.RegisterGlobal<IService, MultipleDependencyService>();
            parentContext.RegisterGlobal<MultipleInterfaceClass>().AsImplementedInterfaces();

            using var parentScope = parentContext.Build();

            var parentPackage = parentScope.Resolver.Resolve<IServiceBundle<IService>>().Package;

            Assert.That(parentPackage.Count, Is.EqualTo(injectionCount + 1));

            var childContext = parentScope.CreateContext();

            childContext.RegisterGlobal<IService, NoDependencyService>();

            using var childScope = childContext.Build();

            var childPackage = childScope.Resolver.Resolve<IServiceBundle<IService>>().Package;

            Assert.That(childPackage.Count, Is.EqualTo(injectionCount + 1));
        }

        [Test]
        public void CannotEnableFieldInjectionWithoutDependencies()
        {
            Assert.That(() =>
            {
                var context = new DependencyInjectionContext();
                context.RegisterTemporal<NoDependencyClass>().WithFieldsInjected();

                _ = context.Build();
            }, Throws.TypeOf<Exception>());

            Assert.That(() =>
            {
                var context = new DependencyInjectionContext();

                context.RegisterTemporalInstance<IService>(() => new DependencyIntoInstance())
                    .WithFieldsInjected();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotEnablePropertyInjectionWithoutDependencies()
        {
            Assert.That(() =>
            {
                var context = new DependencyInjectionContext();
                context.RegisterTemporal<NoDependencyClass>().WithPropertiesInjected();

                _ = context.Build();
            }, Throws.TypeOf<Exception>());

            Assert.That(() =>
            {
                var context = new DependencyInjectionContext();

                context.RegisterTemporalInstance<IService>(() => new DependencyIntoInstance())
                    .WithPropertiesInjected();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotEnableMethodInjectionWithoutDependencies()
        {
            Assert.That(() =>
            {
                var context = new DependencyInjectionContext();
                context.RegisterTemporal<NoDependencyClass>().WithMethodInjected();

                _ = context.Build();
            }, Throws.TypeOf<Exception>());

            Assert.That(() =>
            {
                var context = new DependencyInjectionContext();

                context.RegisterTemporalInstance<IService>(() => new DependencyIntoInstance())
                    .WithMethodInjected();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotBuildWithCircularDependency()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<CircularDependencyClass1>();
            context.RegisterTemporal<CircularDependencyClass2>();

            using var scope = context.Build();

            var childContext = scope.CreateContext();

            childContext.RegisterTemporal<CircularDependencyClass3>();

            Assert.That(() =>
            {
                _ = childContext.Build();

            }, Throws.TypeOf<Exception>());

            Assert.That(() =>
            {
                _ = new DependencyInjectionContext().Build();

            }, Throws.Nothing);
        }
    }
}
