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

            parentContext.Register<NoDependencyService>(Lifetime.Temporal);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyService>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateLocalObjectPerLocalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyService>(Lifetime.Local);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyService>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInstantiateGlobalObjectPerGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<NoDependencyService>(Lifetime.Global);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyService>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveInstanceAlreadyCreated(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyService();

            context.RegisterInstance(instance);

            using var parentScope = context.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyService>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.EqualTo(instance));
            Assert.That(instance2, Is.EqualTo(instance));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance3, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveTemporalInstancePerRequest(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.RegisterInstance(() => new NoDependencyService(), Lifetime.Temporal);

            var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyService>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));

            childScope.Dispose();
            parentScope.Dispose();

            Assert.That(instance3.IsDisposed, Is.False);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveLocalInstancePerLocalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.RegisterInstance(() => new NoDependencyService(), Lifetime.Local);
            var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyService>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.EqualTo(instance2));

            var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));

            childScope.Dispose();
            parentScope.Dispose();

            Assert.That(instance3.IsDisposed, Is.False);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveGlobalInstancePerGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.RegisterInstance(() => new NoDependencyService(), Lifetime.Global);

            var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<NoDependencyService>();
            var instance2 = parentScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.EqualTo(instance2));

            var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<NoDependencyService>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));

            childScope.Dispose();
            parentScope.Dispose();

            Assert.That(instance3.IsDisposed, Is.False);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveObjectAsInheritedType(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyService>(Lifetime.Global).As<IService>();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IDisposable>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            var service = scope.Resolver.Resolve<IService>();

            Assert.That(service is NoDependencyService, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveObjectAsImplementedInterfaces(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyService>(Lifetime.Global).AsImplementedInterfaces();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            var service = scope.Resolver.Resolve<IService>();
            var disposable = scope.Resolver.Resolve<IDisposable>();

            Assert.That(service is NoDependencyService, Is.True);
            Assert.That(disposable is NoDependencyService, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveObjectAsInheritedAndOwnType(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyService>(Lifetime.Global).As<IService>().AsOwnSelf();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IDisposable>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            var service = scope.Resolver.Resolve<IService>();

            Assert.That(service is NoDependencyService, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveObjectAsImplementedInterfacesAndOwnType(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyService>(Lifetime.Global).AsImplementedInterfaces().AsOwnSelf();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.Nothing);

            var service = scope.Resolver.Resolve<IService>();
            var disposable = scope.Resolver.Resolve<IDisposable>();

            Assert.That(service is NoDependencyService, Is.True);
            Assert.That(disposable is NoDependencyService, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldDisposeResolvedObjectWhenDisposed(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Register<NoDependencyService>(Lifetime.Global).As<IDisposable>().AsOwnSelf();

            var scope = context.CreateScope();

            var service = scope.Resolver.Resolve<NoDependencyService>();

            scope.Dispose();

            Assert.That(service.IsDisposed, Is.True);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveDependencyFromGlobalScope(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            parentContext.Register<DualInterface1>(Lifetime.Temporal).AsImplementedInterfaces();

            using var parentScope = parentContext.CreateScope();

            var childContext = parentScope.CreateContext();

            childContext.Register<DualInterface2>(Lifetime.Temporal).AsImplementedInterfaces();

            using var childScope = childContext.CreateScope();

            var grandChildContext = childScope.CreateContext();

            grandChildContext.Register<MultipleDependencyService>(Lifetime.Temporal);

            using var grandChildScope = grandChildContext.CreateScope();

            Assert.That(() =>
            {
                grandChildScope.Resolver.Resolve<MultipleDependencyService>();
            }, Throws.Nothing);
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

            context.Register<ConstructorInjectable>(Lifetime.Global).WithArgument(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<ConstructorInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectInstanceIntoField(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.Register<FieldInjectable>(Lifetime.Global).WithField(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<FieldInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectInstanceIntoProperty(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.Register<PropertyInjectable>(Lifetime.Global).WithProperty(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<PropertyInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldInjectInstanceIntoMethod(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var instance = new NoDependencyClass();

            context.Register<MethodInjectable>(Lifetime.Global).WithMethod(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<MethodInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldRegisterInstallationInRootContext(ISolver solver)
        {
            var context = new DependencyContext(solver);

            context.Install<NoDependencyClassInstallation>();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                scope.Resolver.Resolve<NoDependencyClass>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldRegisterInstallationInChildContext(ISolver solver)
        {
            var parentContext = new DependencyContext(solver);

            using var parentScope = parentContext.CreateScope();

            var childContext = parentScope.CreateContext();

            childContext.Install<NoDependencyClassInstallation>();

            using var childScope = childContext.CreateScope();

            Assert.That(() =>
            {
                childScope.Resolver.Resolve<NoDependencyClass>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(SolverList))]
        public void ShouldResolveFromSubContainer(ISolver solver)
        {
            var context = new DependencyContext(solver);

            var service = default(NoDependencyService);

            context.RegisterFromSubContainer<MultipleDependencyService>(container =>
            {
                container.Register<NoDependencyService>(Lifetime.Global);
                container.Register(resolver => service = resolver.Resolve<NoDependencyService>());
                container.Register<MultipleInterfaceClass>(Lifetime.Global).AsImplementedInterfaces().AsOwnSelf();
                container.Register<MultipleDependencyService>(Lifetime.Global);
            });

            var scope = context.CreateScope();

            Assert.That(() =>
            {
                scope.Resolver.Resolve<MultipleDependencyService>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                scope.Resolver.Resolve<MultipleInterfaceClass>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            scope.Dispose();

            Assert.That(service!.IsDisposed, Is.True);
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
