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

        private static object[] OperationList { get; } =
        {
            ReflectionToOperate.Instance,
            ExpressionToOperate.Instance,
        };

        private static object[] LifetimeList { get; } =
        {
            Lifetime.Temporal,
            Lifetime.Local,
            Lifetime.Global,
        };

        private static object[] OwnershipList { get; } =
        {
            Ownership.Internal,
            Ownership.External,
        };

        private static IEnumerable<object> OperationAndLifetimeMatrix
        {
            get
            {
                foreach (var operation in OperationList)
                {
                    foreach (var lifetime in LifetimeList)
                    {
                        yield return new[] { operation, lifetime };
                    }
                }
            }
        }

        private static IEnumerable<object> OperationAndOwnershipMatrix
        {
            get
            {
                foreach (var operation in OperationList)
                {
                    foreach (var ownership in OwnershipList)
                    {
                        yield return new[] { operation, ownership };
                    }
                }
            }
        }

        private static IEnumerable<object> OperationAndLifetimeAndOwnershipMatrix
        {
            get
            {
                foreach (var operation in OperationList)
                {
                    foreach (var lifetime in LifetimeList)
                    {
                        foreach (var ownership in OwnershipList)
                        {
                            yield return new[] { operation, lifetime, ownership };
                        }
                    }
                }
            }
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldResolveResolver(IDependencyOperation operation)
        {
            using var scope = new DependencyContext(operation).CreateScope();

            var resolver = scope.Resolver.Resolve<IObjectResolver>();

            Assert.That(resolver, Is.EqualTo(scope.Resolver));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldResolveObject(IDependencyOperation operation, Lifetime lifetime)
        {
            var parentContext = new DependencyContext(operation);

            parentContext.Register<object>(lifetime);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<object>();
            var instance2 = parentScope.Resolver.Resolve<object>();

            var childContext = parentScope.CreateContext();
            using var childScope = childContext.CreateScope();

            var instance3 = childScope.Resolver.Resolve<object>();

            Assert.That(instance1 == instance2, Is.EqualTo(lifetime == Lifetime.Local || lifetime ==  Lifetime.Global));
            Assert.That(instance2 == instance3, Is.EqualTo(lifetime == Lifetime.Global));
            Assert.That(instance3 == instance1, Is.EqualTo(lifetime == Lifetime.Global));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldDisposeResolvedObjectWhenHasDisposed(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            context.Register<IndependentDisposable>(lifetime);

            var scope = context.CreateScope();

            var instance = scope.Resolver.Resolve<IndependentDisposable>();

            scope.Dispose();

            Assert.That(instance.IsDisposed, Is.EqualTo(lifetime != Lifetime.Temporal));
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldResolveInstance(IDependencyOperation operation)
        {
            var parentContext = new DependencyContext(operation);

            var expected = new object();

            parentContext.RegisterInstance(expected);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<object>();
            var instance2 = parentScope.Resolver.Resolve<object>();

            var childContext = parentScope.CreateContext();
            using var childScope = childContext.CreateScope();

            var instance3 = childScope.Resolver.Resolve<object>();

            Assert.That(instance1, Is.EqualTo(expected));
            Assert.That(instance2, Is.EqualTo(expected));
            Assert.That(instance3, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldNotDisposeInstanceWhenHasDisposed(IDependencyOperation operation)
        {
            var context = new DependencyContext(operation);

            var expected = new IndependentDisposable();

            context.RegisterInstance(expected);

            var scope = context.CreateScope();

            var instance = scope.Resolver.Resolve<IndependentDisposable>();

            Assert.That(instance, Is.EqualTo(expected));

            scope.Dispose();

            Assert.That(instance.IsDisposed, Is.False);
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldCreateObject(IDependencyOperation operation)
        {
            var parentContext = new DependencyContext(operation);

            parentContext.Register(CreateObject.Instance);

            using var parentScope = parentContext.CreateScope();

            var instance1 = parentScope.Resolver.Resolve<object>();
            var instance2 = parentScope.Resolver.Resolve<object>();

            var childContext = parentScope.CreateContext();
            using var childScope = childContext.CreateScope();

            var instance3 = childScope.Resolver.Resolve<object>();

            Assert.That(instance1, Is.EqualTo(instance2));
            Assert.That(instance2, Is.EqualTo(instance3));
            Assert.That(instance3, Is.EqualTo(instance1));
        }

        [TestCaseSource(nameof(OperationAndOwnershipMatrix))]
        public void ShouldManageCreatedObjectWhenHasDisposed(IDependencyOperation operation, Ownership ownership)
        {
            var context = new DependencyContext(operation);

            context.Register(() => new IndependentDisposable(), ownership);

            var scope = context.CreateScope();

            var instance = scope.Resolver.Resolve<IndependentDisposable>();

            scope.Dispose();

            Assert.That(instance.IsDisposed, Is.EqualTo(ownership is Ownership.Internal));
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldResolveObjectImmediatelyJustAfterCreatingScope(IDependencyOperation operation)
        {
            var context = new DependencyContext(operation);

            var executed = false;
            context.Register(() =>
            {
                executed = true;
                return new IndependentDisposable();
            }).ResolvedImmediately().As<IDisposable>();

            using var scope = context.CreateScope();

            Assert.That(executed, Is.True);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldResolveObjectAsInheritedType(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            context.Register<NoDependencyService>(lifetime).As<IService>();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IDisposable>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IService>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldResolveObjectAsImplementedInterfaces(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            context.Register<NoDependencyService>(lifetime).AsImplementedInterfaces();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IDisposable>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IService>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldResolveObjectAsInheritedAndOwnType(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            context.Register<NoDependencyService>(lifetime).As<IService>().AsOwnSelf();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IDisposable>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IService>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldResolveObjectAsImplementedInterfacesAndOwnType(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            context.Register<NoDependencyService>(lifetime).AsImplementedInterfaces().AsOwnSelf();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyService>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IDisposable>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IService>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldResolveDependencyFromGlobalScope(IDependencyOperation operation, Lifetime lifetime)
        {
            var parentContext = new DependencyContext(operation);

            parentContext.Register<DualInterface1>(lifetime).AsImplementedInterfaces();

            using var parentScope = parentContext.CreateScope();

            var childContext = parentScope.CreateContext();

            childContext.Register<DualInterface2>(lifetime).AsImplementedInterfaces();

            using var childScope = childContext.CreateScope();

            var grandChildContext = childScope.CreateContext();

            grandChildContext.Register<MultipleDependencyService>(lifetime);

            using var grandChildScope = grandChildContext.CreateScope();

            Assert.That(() =>
            {
                _ = grandChildScope.Resolver.Resolve<MultipleDependencyService>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldResolveCollection(IDependencyOperation operation)
        {
            var parentInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyContext(operation);

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
            var totalParentPackageAmount = parentInjectionCount + 1;

            var parentArray = parentScope.Resolver.Resolve<IService[]>();
            var parentReadOnlyList = parentScope.Resolver.Resolve<IReadOnlyList<IService>>();
            var parentReadOnlyCollection = parentScope.Resolver.Resolve<IReadOnlyCollection<IService>>();
            var parentEnumerable = parentScope.Resolver.Resolve<IEnumerable<IService>>();
            var parentPackage = parentScope.Resolver.Resolve<ServiceBundle<IService>>().Package;

            Assert.That(parentArray.Length, Is.EqualTo(totalParentInjectionAmount));
            Assert.That(parentReadOnlyList.Count, Is.EqualTo(totalParentInjectionAmount));
            Assert.That(parentReadOnlyCollection.Count, Is.EqualTo(totalParentInjectionAmount));
            Assert.That(parentEnumerable.Count(), Is.EqualTo(totalParentInjectionAmount));
            Assert.That(parentPackage.Count, Is.EqualTo(totalParentPackageAmount));

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
            var childPackage = childScope.Resolver.Resolve<ServiceBundle<IService>>().Package;

            var totalChildPackageAmount = parentInjectionCount + childInjectionCount;
            var totalChildInjectionAmount = parentInjectionCount + childInjectionCount + 1;

            Assert.That(childArray.Length, Is.EqualTo(totalChildInjectionAmount));
            Assert.That(childReadOnlyList.Count, Is.EqualTo(totalChildInjectionAmount));
            Assert.That(childReadOnlyCollection.Count, Is.EqualTo(totalChildInjectionAmount));
            Assert.That(childEnumerable.Count(), Is.EqualTo(totalChildInjectionAmount));
            Assert.That(childPackage.Count, Is.EqualTo(totalChildPackageAmount));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldInjectDependencyIntoField(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var instance = new object();

            context.RegisterInstance(instance);
            context.Register<FieldInjectable>(lifetime).WithFieldInjection();

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<FieldInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldInjectDependencyIntoProperty(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var instance = new object();

            context.RegisterInstance(instance);
            context.Register<PropertyInjectable>(lifetime).WithPropertyInjection();

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<PropertyInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldInjectDependencyIntoMethod(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var instance = new object();

            context.RegisterInstance(instance);
            context.Register<MethodInjectable>(lifetime).WithMethodInjection();

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<MethodInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldInjectInstance(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var instance = new object();

            context.Register<ConstructorInjectable>(lifetime).WithArgument(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<ConstructorInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldInjectInstanceIntoField(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var instance = new object();

            context.Register<FieldInjectable>(lifetime).WithField(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<FieldInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldInjectInstanceIntoProperty(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var instance = new object();

            context.Register<PropertyInjectable>(lifetime).WithProperty(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<PropertyInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldInjectInstanceIntoMethod(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var instance = new object();

            context.Register<MethodInjectable>(lifetime).WithMethod(instance);

            using var scope = context.CreateScope();

            var resolved = scope.Resolver.Resolve<MethodInjectable>();

            Assert.That(resolved.Instance, Is.EqualTo(instance));
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldRegisterInstallationInRootContext(IDependencyOperation operation)
        {
            var context = new DependencyContext(operation);

            context.Install<IndependentInstallation>();

            using var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<object>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationList))]
        public void ShouldRegisterInstallationInChildContext(IDependencyOperation operation)
        {
            var parentContext = new DependencyContext(operation);
            parentContext.RegisterInstance(0);

            using var parentScope = parentContext.CreateScope();

            var childContext = parentScope.CreateContext();

            childContext.Install<DependentInstallation>();

            using var childScope = childContext.CreateScope();

            Assert.That(() =>
            {
                _ = childScope.Resolver.Resolve<object>();
            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldResolveFromSubContainer(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            var installation = new ObjectDependentDisposableInstallation(lifetime);

            context.RegisterFromSubContainer<ObjectDependentDisposable>(installation);

            var scope = context.CreateScope();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<ObjectDependentDisposable>();
            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<object>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            scope.Dispose();

            Assert.That(installation.Disposable!.IsDisposed, Is.EqualTo(lifetime != Lifetime.Temporal));
        }

        [TestCaseSource(nameof(OperationAndOwnershipMatrix))]
        public void ShouldResolveFactory(IDependencyOperation operation, Ownership ownership)
        {
            var context = new DependencyContext(operation);

            var installation = new IndependentDisposableInstallation();

            context.RegisterFactory<IndependentDisposable>(installation, ownership);

            var scope = context.CreateScope();

            var factory = scope.Resolver.Resolve<IFactory<IndependentDisposable>>();

            var instance = factory.Create();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<IndependentDisposable>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            scope.Dispose();

            Assert.That(instance.IsDisposed, Is.EqualTo(ownership is Ownership.Internal));
        }

        [TestCaseSource(nameof(OperationAndLifetimeAndOwnershipMatrix))]
        public void ShouldResolveFactoryToCreateOutputFromInput(IDependencyOperation operation, Lifetime lifetime, Ownership ownership)
        {
            var context = new DependencyContext(operation);

            var installation = new DisposableOutputInstallation(lifetime);

            context.RegisterFactory<object, DisposableOutput>(installation, ownership);

            var scope = context.CreateScope();

            var factory = scope.Resolver.Resolve<IFactory<object, DisposableOutput>>();

            var input = new object();

            var output = factory.Create(input);

            Assert.That(output.Instance, Is.EqualTo(input));

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<DisposableOutput>();
            }, Throws.TypeOf<RagnarokNotRegisteredException>());

            scope.Dispose();

            Assert.That(output.IsDisposed, Is.EqualTo(ownership is Ownership.Internal && lifetime != Lifetime.Temporal));
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldCreateScopeWithoutCircularDependency(IDependencyOperation operation, Lifetime lifetime)
        {
            var parentContext = new DependencyContext(operation);
            parentContext.Register<CircularDependencyClass1>(lifetime);

            using var parentScope = parentContext.CreateScope();

            var childContext1 = parentScope.CreateContext();
            childContext1.Register<CircularDependencyClass2>(lifetime);

            var childContext2 = parentScope.CreateContext();
            childContext2.Register<CircularDependencyClass3>(lifetime);

            Assert.That(() =>
            {
                _ = childContext1.CreateScope();

            }, Throws.Nothing);

            Assert.That(() =>
            {
                _ = childContext2.CreateScope();

            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldDetectCircularDependencyInLocalScope(IDependencyOperation operation, Lifetime lifetime)
        {
            var context = new DependencyContext(operation);

            context.Register<CircularDependencyClass1>(lifetime);
            context.Register<CircularDependencyClass2>(lifetime);
            context.Register<CircularDependencyClass3>(lifetime);

            Assert.That(() =>
            {
                _ = context.CreateScope();

            }, Throws.TypeOf<RagnarokCircularDependencyException>());

            Assert.That(() =>
            {
                _ = new DependencyContext(operation).CreateScope();

            }, Throws.Nothing);
        }

        [TestCaseSource(nameof(OperationAndLifetimeMatrix))]
        public void ShouldDetectCircularDependencyInGlobalScope(IDependencyOperation operation, Lifetime lifetime)
        {
            var parentContext = new DependencyContext(operation);

            parentContext.Register<CircularDependencyClass1>(lifetime);

            using var parentScope = parentContext.CreateScope();

            var childContext = parentScope.CreateContext();

            childContext.Register<CircularDependencyClass2>(lifetime);

            using var childScope = childContext.CreateScope();

            var grandchildContext = childScope.CreateContext();

            grandchildContext.Register<CircularDependencyClass3>(lifetime);

            Assert.That(() =>
            {
                _ = grandchildContext.CreateScope();

            }, Throws.TypeOf<RagnarokCircularDependencyException>());
        }
    }
}
