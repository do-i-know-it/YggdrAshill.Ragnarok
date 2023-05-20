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

            parentContext.RegisterTemporal<InjectedClass>();

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<InjectedClass>();
            var instance2 = parentScope.Resolver.Resolve<InjectedClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<InjectedClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        [Test]
        public void ShouldInstantiateLocalObjectPerScope()
        {
            var parentContext = new DependencyInjectionContext();

            parentContext.RegisterLocal<InjectedClass>();

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<InjectedClass>();
            var instance2 = parentScope.Resolver.Resolve<InjectedClass>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<InjectedClass>();

            Assert.That(instance1, Is.Not.EqualTo(instance3));
            Assert.That(instance2, Is.Not.EqualTo(instance3));
        }

        // TODO: Rename method.
        // TODO: Modify implementation of specification.
        [Test]
        public void ShouldInstantiateGlobalObjectPerService()
        {
            var parentContext = new DependencyInjectionContext();

            parentContext.RegisterGlobal<InjectedStruct>().WithArgument("value", new Random().Next());

            using var parentScope = parentContext.Build();

            var instance1 = parentScope.Resolver.Resolve<InjectedStruct>();
            var instance2 = parentScope.Resolver.Resolve<InjectedStruct>();

            Assert.That(instance1, Is.EqualTo(instance2));

            using var childScope = parentScope.CreateChildScope();

            var instance3 = childScope.Resolver.Resolve<InjectedStruct>();

            Assert.That(instance1, Is.EqualTo(instance3));
            Assert.That(instance2, Is.EqualTo(instance3));
        }

        // TODO: Modify implementation of specification.
        [Test]
        public void ShouldResolveRegisteredInstance()
        {
            var context = new DependencyInjectionContext();

            var instance = new InjectedClass();

            context.RegisterInstance<IInjectedInterface1>(instance);

            using var scope = context.Build();

            var resolved = scope.Resolver.Resolve<IInjectedInterface1>();

            Assert.That(resolved, Is.EqualTo(instance));
        }

        [Test]
        public void ShouldInstantiateRegisteredAsInterface()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>()
                .As<IInjectedInterface1>()
                .And<IInjectedInterface2>()
                .And<IInjectedInterface3>();

            using var scope = context.Build();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<InjectedClass>();
            }, Throws.TypeOf<Exception>());

            var instance1 = scope.Resolver.Resolve<IInjectedInterface1>();
            var instance2 = scope.Resolver.Resolve<IInjectedInterface2>();
            var instance3 = scope.Resolver.Resolve<IInjectedInterface3>();

            Assert.That(instance1 is InjectedClass, Is.True);
            Assert.That(instance2 is InjectedClass, Is.True);
            Assert.That(instance3 is InjectedClass, Is.True);
        }

        [Test]
        public void ShouldInstantiateRegisteredAsImplementedInterfaces()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>().AsImplementedInterfaces();

            using var scope = context.Build();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<InjectedClass>();
            }, Throws.TypeOf<Exception>());

            var instance1 = scope.Resolver.Resolve<IInjectedInterface1>();
            var instance2 = scope.Resolver.Resolve<IInjectedInterface2>();
            var instance3 = scope.Resolver.Resolve<IInjectedInterface3>();

            Assert.That(instance1 is InjectedClass, Is.True);
            Assert.That(instance2 is InjectedClass, Is.True);
            Assert.That(instance3 is InjectedClass, Is.True);
        }

        [Test]
        public void ShouldInstantiateRegisteredAsInterfaceAndSelf()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>()
                .As<IInjectedInterface1>()
                .And<IInjectedInterface2>()
                .And<IInjectedInterface3>()
                .AndSelf();

            using var scope = context.Build();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<InjectedClass>();
            }, Throws.Nothing);

            var instance1 = scope.Resolver.Resolve<IInjectedInterface1>();
            var instance2 = scope.Resolver.Resolve<IInjectedInterface2>();
            var instance3 = scope.Resolver.Resolve<IInjectedInterface3>();

            Assert.That(instance1 is InjectedClass, Is.True);
            Assert.That(instance2 is InjectedClass, Is.True);
            Assert.That(instance3 is InjectedClass, Is.True);
        }

        [Test]
        public void ShouldInstantiateRegisteredAsImplementedInterfacesAndSelf()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>().AsImplementedInterfaces().AndSelf();

            using var scope = context.Build();

            Assert.That(() =>
            {
                _ = scope.Resolver.Resolve<InjectedClass>();
            }, Throws.Nothing);

            var instance1 = scope.Resolver.Resolve<IInjectedInterface1>();
            var instance2 = scope.Resolver.Resolve<IInjectedInterface2>();
            var instance3 = scope.Resolver.Resolve<IInjectedInterface3>();

            Assert.That(instance1 is InjectedClass, Is.True);
            Assert.That(instance2 is InjectedClass, Is.True);
            Assert.That(instance3 is InjectedClass, Is.True);
        }

        [Test]
        public void ShouldInjectDependenciesIntoConstructor()
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<InjectedClass>();
            context.RegisterLocal<InjectedStruct>()
                .WithArgument("value", new Random().Next());
            context.RegisterTemporal<ConstructorInjectableClass>();

            using var scope = context.Build();

            var injectedClass = scope.Resolver.Resolve<InjectedClass>();
            var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
            var instance = scope.Resolver.Resolve<ConstructorInjectableClass>();

            Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
            Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
        }

        [Test]
        public void ShouldInjectDependenciesIntoInstanceAfterEnabled()
        {
            var context = new DependencyInjectionContext();

            var constructorInjected = new InjectedStruct(new Random().Next());
            var fieldInjected = new InjectedStruct(new Random().Next());
            var propertyInjected = new InjectedStruct(new Random().Next());
            var methodInjected = new InjectedStruct(new Random().Next());

            context.RegisterGlobal<AllInjectableClass>()
                .WithArgument("constructorInjected", constructorInjected)
                .WithFieldInjection()
                .With("fieldInjected", fieldInjected)
                .WithPropertyInjection()
                .With("PropertyInjected", propertyInjected)
                .WithMethodInjection()
                .With("methodInjected", methodInjected);

            using var scope = context.Build();

            var instance = scope.Resolver.Resolve<AllInjectableClass>();

            Assert.That(instance.ConstructorInjected, Is.EqualTo(constructorInjected));
            Assert.That(instance.FieldInjected, Is.EqualTo(fieldInjected));
            Assert.That(instance.PropertyInjected, Is.EqualTo(propertyInjected));
            Assert.That(instance.MethodInjected, Is.EqualTo(methodInjected));
        }

        [Test]
        public void ShouldResolveArray()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterTemporal<InjectedClass>();
            }

            using var parentScope = parentContext.Build();

            var parentArray = parentScope.Resolver.Resolve<InjectedClass[]>();

            Assert.That(parentArray.Length, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterTemporal<InjectedClass>();
            }

            using var childScope = childContext.Build();

            var childArray = childScope.Resolver.Resolve<InjectedClass[]>();

            Assert.That(childArray.Length, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveReadOnlyList()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterTemporal<InjectedClass>();
            }

            using var parentScope = parentContext.Build();

            var parentReadOnlyList = parentScope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();

            Assert.That(parentReadOnlyList.Count, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterTemporal<InjectedClass>();
            }

            using var childScope = childContext.Build();

            var childReadOnlyList = childScope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();

            Assert.That(childReadOnlyList.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveReadOnlyCollection()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterTemporal<InjectedClass>();
            }

            using var parentScope = parentContext.Build();

            var parentReadOnlyCollection = parentScope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();

            Assert.That(parentReadOnlyCollection.Count, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterTemporal<InjectedClass>();
            }

            using var childScope = childContext.Build();

            var childReadOnlyCollection = childScope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();

            Assert.That(childReadOnlyCollection.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveEnumerable()
        {
            var parentInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var parentContext = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                parentContext.RegisterTemporal<InjectedClass>();
            }

            using var parentScope = parentContext.Build();

            var parentEnumerable = parentScope.Resolver.Resolve<IEnumerable<InjectedClass>>();

            Assert.That(parentEnumerable.Count, Is.EqualTo(parentInjectionAmount));

            var childInjectionAmount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);

            var childContext = parentScope.CreateContext();

            for (var count = 0; count < childInjectionAmount; count++)
            {
                childContext.RegisterTemporal<InjectedClass>();
            }

            using var childScope = childContext.Build();

            var childEnumerable = childScope.Resolver.Resolve<IEnumerable<InjectedClass>>();

            Assert.That(childEnumerable.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
        }

        [Test]
        public void ShouldResolveLocalInstanceList()
        {
            var parentLocalInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);
            var parentValue = 1;

            var context = new DependencyInjectionContext();

            for (var count = 0; count < parentLocalInjectionCount; count++)
            {
                context.RegisterLocal<InjectedStruct>().WithArgument("value", parentValue + count);
            }

            context.RegisterGlobal<InjectedStruct>().WithArgument("value", 0);

            using var scope = context.Build();

            var parentLocalInstanceList = scope.Resolver.Resolve<ILocalInstanceList<InjectedStruct>>();

            Assert.That(parentLocalInstanceList.InstanceList.Count, Is.EqualTo(parentLocalInjectionCount + 1));

            var childTemporalInjectionCount = new Random().Next(MinMultipleInjectionCount, MaxMultipleInjectionCount);
            var childValue = parentLocalInjectionCount + 1;

            var childContext = scope.CreateContext();

            for (var count = 0; count < childTemporalInjectionCount; count++)
            {
                childContext.RegisterTemporal<InjectedStruct>().WithArgument("value", childValue + count);
            }

            using var childScope = childContext.Build();

            var childLocalInstanceList = childScope.Resolver.Resolve<ILocalInstanceList<InjectedStruct>>();

            Assert.That(childLocalInstanceList.InstanceList.Count, Is.EqualTo(parentLocalInjectionCount + childTemporalInjectionCount));
        }

        [Test]
        public void CannotEnableFieldInjectionWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>().WithFieldInjection();

            Assert.That(() =>
            {
                _ = context.Build();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotEnablePropertyInjectionWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>().WithPropertyInjection();

            Assert.That(() =>
            {
                _ = context.Build();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotEnableMethodInjectionWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>().WithMethodInjection();

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
                _ = scope.Resolver.Resolve<InjectedClass>();
            }, Throws.TypeOf<Exception>());
        }
    }
}
