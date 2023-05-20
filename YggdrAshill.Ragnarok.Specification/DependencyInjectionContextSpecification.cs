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
        private const int MaxMultipleInjectionCount = 3;

        [Test]
        public void ShouldResolveResolver()
        {
            using var scope = new DependencyInjectionContext().Build();

            var resolver = scope.Resolver.Resolve<IResolver>();

            Assert.That(resolver, Is.EqualTo(scope.Resolver));
        }

        [Test]
        public void ShouldInstantiateTemporalObjectPerRequest()
        {
            var parentContext = new DependencyInjectionContext();

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

        [Test]
        public void ShouldInstantiateLocalObjectPerScope()
        {
            var parentContext = new DependencyInjectionContext();

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

        // TODO: Rename method.
        [Test]
        public void ShouldInstantiateGlobalObjectPerService()
        {
            var parentContext = new DependencyInjectionContext();

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

        // TODO: integrate into any other specifications.
        [Test]
        public void ShouldInjectDependenciesIntoConstructor()
        {
            var context = new DependencyInjectionContext();

            context.RegisterGlobal<NoDependencyClass>();
            context.RegisterTemporal<DependencyIntoConstructor>();

            using var scope = context.Build();

            var noDependencyClass = scope.Resolver.Resolve<NoDependencyClass>();
            var resolved = scope.Resolver.Resolve<DependencyIntoConstructor>();

            Assert.That(resolved.NoDependencyClass, Is.EqualTo(noDependencyClass));
        }

        [Test]
        public void ShouldInjectDependenciesIntoInstanceAfterEnabled()
        {
            var context = new DependencyInjectionContext();

            var fieldInjected = new NoDependencyClass();
            var propertyInjected = new NoDependencyClass();
            var methodInjected = new NoDependencyClass();

            context.RegisterGlobal<DependencyIntoInstance>()
                .WithFieldInjection()
                .With("fieldInjected", fieldInjected)
                .WithPropertyInjection()
                .With("PropertyInjected", propertyInjected)
                .WithMethodInjection()
                .With("methodInjected", methodInjected);

            using var scope = context.Build();

            var resolved = scope.Resolver.Resolve<DependencyIntoInstance>();

            Assert.That(resolved.FieldInjected, Is.EqualTo(fieldInjected));
            Assert.That(resolved.PropertyInjected, Is.EqualTo(propertyInjected));
            Assert.That(resolved.MethodInjected, Is.EqualTo(methodInjected));
        }

        [Test]
        public void ShouldResolveRegisteredInstance()
        {
            var context = new DependencyInjectionContext();

            var instance = new NoDependencyService();

            context.RegisterInstance<IService>(instance);

            using var scope = context.Build();

            var resolved = scope.Resolver.Resolve<IService>();

            Assert.That(resolved, Is.EqualTo(instance));
        }

        // TODO: Rename method.
        [Test]
        public void ShouldInstantiateRegisteredAsInterface()
        {
            var context = new DependencyInjectionContext();

            context.RegisterGlobal<MultipleInterfaceClass>()
                .As<IInterfaceA>()
                .And<IInterfaceB>()
                .And<IInterfaceC>();

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
                .And<IInterfaceB>()
                .And<IInterfaceC>()
                .AndSelf();

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

            context.RegisterTemporal<MultipleInterfaceClass>().AsImplementedInterfaces().AndSelf();

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

        [Test]
        public void ShouldResolveArray()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var parentScope = parentContext.Build();

            var parentArray = parentScope.Resolver.Resolve<IService[]>();

            Assert.That(parentArray.Length, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var childScope = childContext.Build();

            var childArray = childScope.Resolver.Resolve<IService[]>();

            Assert.That(childArray.Length, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveReadOnlyList()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var parentScope = parentContext.Build();

            var parentReadOnlyList = parentScope.Resolver.Resolve<IReadOnlyList<IService>>();

            Assert.That(parentReadOnlyList.Count, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var childScope = childContext.Build();

            var childReadOnlyList = childScope.Resolver.Resolve<IReadOnlyList<IService>>();

            Assert.That(childReadOnlyList.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveReadOnlyCollection()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var parentScope = parentContext.Build();

            var parentReadOnlyCollection = parentScope.Resolver.Resolve<IReadOnlyCollection<IService>>();

            Assert.That(parentReadOnlyCollection.Count, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var childScope = childContext.Build();

            var childReadOnlyCollection = childScope.Resolver.Resolve<IReadOnlyCollection<IService>>();

            Assert.That(childReadOnlyCollection.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveEnumerable()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var parentScope = parentContext.Build();

            var parentEnumerable = parentScope.Resolver.Resolve<IEnumerable<IService>>();

            Assert.That(parentEnumerable.Count, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterGlobal<NoDependencyService>().As<IService>();
            }

            using var childScope = childContext.Build();

            var childEnumerable = childScope.Resolver.Resolve<IEnumerable<IService>>();

            Assert.That(childEnumerable.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveLocalInstanceList()
        {
            var injectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < injectionCount; count++)
            {
                if (count % 2 == 0)
                {
                    parentContext.RegisterLocal<NoDependencyService>().As<IService>();
                }
                else
                {
                    parentContext.RegisterTemporal<NoDependencyService>().As<IService>();
                }
            }

            parentContext.RegisterGlobal<SingleDependencyService>().As<IService>();
            parentContext.RegisterGlobal<DualInterface1>().AsImplementedInterfaces();

            using var parentScope = parentContext.Build();

            var parentLocalInstanceList = parentScope.Resolver.Resolve<ILocalInstanceList<IService>>();

            Assert.That(parentLocalInstanceList.InstanceList.Count, Is.EqualTo(injectionCount + 1));

            var childContext = parentScope.CreateContext();

            childContext.RegisterGlobal<MultipleDependencyService>().As<IService>();
            childContext.RegisterGlobal<DualInterface2>().AsImplementedInterfaces();

            using var childScope = childContext.Build();

            var childLocalInstanceList = childScope.Resolver.Resolve<ILocalInstanceList<IService>>();

            Assert.That(childLocalInstanceList.InstanceList.Count, Is.EqualTo(injectionCount + 1));
        }

        [Test]
        public void CannotEnableFieldInjectionWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<NoDependencyClass>().WithFieldInjection();

            Assert.That(() =>
            {
                _ = context.Build();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotEnablePropertyInjectionWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<NoDependencyClass>().WithPropertyInjection();

            Assert.That(() =>
            {
                _ = context.Build();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotEnableMethodInjectionWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<NoDependencyClass>().WithMethodInjection();

            Assert.That(() =>
            {
                _ = context.Build();
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

        [Test]
        public void CannotResolveNotRegistered()
        {
            using var scope = new DependencyInjectionContext().Build();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<NoDependencyClass>();
            }, Throws.TypeOf<Exception>());
        }
    }
}
