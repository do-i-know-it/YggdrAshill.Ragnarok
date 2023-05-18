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
        private const int MaxMultipleInjectionCount = 3;

        [Test]
        public void ShouldResolveResolver()
        {
            var context = new DependencyInjectionContext();

            using (var scope = context.Build())
            {
                var resolver = scope.Resolver.Resolve<IResolver>();

                Assert.That(resolver, Is.EqualTo(scope.Resolver));
            }
        }

        [Test]
        public void ShouldInstantiateTemporalObjectPerRequest()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>();

            using (var scope = context.Build())
            {
                var instance1 = scope.Resolver.Resolve<InjectedClass>();
                var instance2 = scope.Resolver.Resolve<InjectedClass>();

                Assert.That(instance1, Is.Not.EqualTo(instance2));

                using (var child = scope.CreateScope())
                {
                    var instance3 = child.Resolver.Resolve<InjectedClass>();

                    Assert.That(instance1, Is.Not.EqualTo(instance3));
                    Assert.That(instance2, Is.Not.EqualTo(instance3));
                }
            }
        }

        [Test]
        public void ShouldInstantiateLocalObjectPerScope()
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<InjectedClass>();

            using (var scope = context.Build())
            {
                var instance1 = scope.Resolver.Resolve<InjectedClass>();
                var instance2 = scope.Resolver.Resolve<InjectedClass>();

                Assert.That(instance1, Is.EqualTo(instance2));

                using (var child = scope.CreateScope())
                {
                    var instance3 = child.Resolver.Resolve<InjectedClass>();

                    Assert.That(instance1, Is.Not.EqualTo(instance3));
                    Assert.That(instance2, Is.Not.EqualTo(instance3));
                }
            }
        }

        [Test]
        public void ShouldInstantiateGlobalObjectPerService()
        {
            var context = new DependencyInjectionContext();

            context.RegisterGlobal<InjectedStruct>().WithArgument("value", new Random().Next());

            using (var scope = context.Build())
            {
                var instance1 = scope.Resolver.Resolve<InjectedStruct>();
                var instance2 = scope.Resolver.Resolve<InjectedStruct>();

                Assert.That(instance1, Is.EqualTo(instance2));

                using (var child = scope.CreateScope())
                {
                    var instance3 = child.Resolver.Resolve<InjectedStruct>();

                    Assert.That(instance1, Is.EqualTo(instance3));
                    Assert.That(instance2, Is.EqualTo(instance3));
                }
            }
        }

        [Test]
        public void ShouldInstantiateRegisteredAsInterface()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>()
                .As<IInjectedInterface1>()
                .And<IInjectedInterface2>()
                .And<IInjectedInterface3>();

            using (var scope = context.Build())
            {
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
        }

        [Test]
        public void ShouldInstantiateRegisteredAsImplementedInterfaces()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>().AsImplementedInterfaces();

            using (var scope = context.Build())
            {
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

            using (var scope = context.Build())
            {
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
        }

        [Test]
        public void ShouldInstantiateRegisteredAsImplementedInterfacesAndSelf()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>().AsImplementedInterfaces().AndSelf();

            using (var scope = context.Build())
            {
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
        }

        [Test]
        public void ShouldInjectDependenciesIntoConstructor()
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<InjectedClass>();
            context.RegisterLocal<InjectedStruct>()
                .WithArgument("value", new Random().Next());
            context.RegisterTemporal<ConstructorInjectableClass>();

            using (var scope = context.Build())
            {
                var injectedClass = scope.Resolver.Resolve<InjectedClass>();
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<ConstructorInjectableClass>();

                Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
                Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
            }
        }

        [Test]
        public void ShouldInjectDependenciesIntoFieldsAfterEnabled()
        {
            var context = new DependencyInjectionContext();

            var injectedClass = new InjectedClass();

            context.RegisterLocal<InjectedStruct>()
                .WithArgument("value", new Random().Next());
            context.RegisterGlobal<FieldInjectableClass>()
                .WithFieldInjection()
                .With("injectedClass", injectedClass);

            using (var scope = context.Build())
            {
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<FieldInjectableClass>();

                Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
                Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
            }
        }

        [Test]
        public void ShouldInjectDependenciesIntoPropertiesAfterEnabled()
        {
            var context = new DependencyInjectionContext();

            var injectedClass = new InjectedClass();

            context.RegisterLocal<InjectedStruct>()
                .WithArgument("value", new Random().Next());
            context.RegisterGlobal<PropertyInjectableClass>()
                .WithPropertyInjection()
                .With($"{nameof(PropertyInjectableClass.InjectedClass)}", injectedClass);

            using (var scope = context.Build())
            {
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<PropertyInjectableClass>();

                Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
                Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
            }
        }

        [Test]
        public void ShouldInjectDependenciesIntoMethodsAfterEnabled()
        {
            var context = new DependencyInjectionContext();

            var injectedClass = new InjectedClass();

            context.RegisterLocal<InjectedStruct>()
                .WithArgument("value", new Random().Next());
            context.RegisterGlobal<MethodInjectableClass>()
                .WithMethodInjection()
                .With("injectedClass", injectedClass);

            using (var scope = context.Build())
            {
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<MethodInjectableClass>();

                Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
                Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
            }
        }

        [Test]
        public void ShouldInjectDependenciesIntoAllAfterEnabled()
        {
            var context = new DependencyInjectionContext();

            var fieldInjected = new InjectedStruct(new Random().Next());
            var propertyInjected = new InjectedStruct(new Random().Next());
            var methodInjected = new InjectedStruct(new Random().Next());

            context.RegisterLocal<InjectedStruct>()
                .WithArgument("value", new Random().Next());
            context.RegisterGlobal<AllInjectableClass>()
                .WithFieldInjection()
                .With("fieldInjected", fieldInjected)
                .WithPropertyInjection()
                .With(nameof(AllInjectableClass.PropertyInjected), propertyInjected)
                .WithMethodInjection()
                .With("methodInjected", methodInjected);

            using (var scope = context.Build())
            {
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<AllInjectableClass>();

                Assert.That(instance.ConstructorInjected, Is.EqualTo(injectedStruct));
                Assert.That(instance.FieldInjected, Is.EqualTo(fieldInjected));
                Assert.That(instance.PropertyInjected, Is.EqualTo(propertyInjected));
                Assert.That(instance.MethodInjected, Is.EqualTo(methodInjected));
            }
        }

        [Test]
        public void ShouldResolveArray()
        {
            var parentInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<InjectedClass[]>();

                Assert.That(injectedClassList.Length, Is.EqualTo(parentInjectionAmount));

                var childInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

                var childContext = scope.CreateContext();

                for (var count = 0; count < childInjectionAmount; count++)
                {
                    childContext.RegisterTemporal<InjectedClass>();
                }

                using (var childScope = childContext.Build())
                {
                    injectedClassList = childScope.Resolver.Resolve<InjectedClass[]>();

                    Assert.That(injectedClassList.Length, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
                }
            }
        }

        [Test]
        public void ShouldResolveReadOnlyList()
        {
            var parentInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();

                Assert.That(injectedClassList.Count, Is.EqualTo(parentInjectionAmount));

                var childInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

                var childContext = scope.CreateContext();

                for (var count = 0; count < childInjectionAmount; count++)
                {
                    childContext.RegisterTemporal<InjectedClass>();
                }

                using (var childScope = childContext.Build())
                {
                    injectedClassList = childScope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();

                    Assert.That(injectedClassList.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
                }
            }
        }

        [Test]
        public void ShouldResolveReadOnlyCollection()
        {
            var parentInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();

                Assert.That(injectedClassList.Count, Is.EqualTo(parentInjectionAmount));

                var childInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

                var childContext = scope.CreateContext();

                for (var count = 0; count < childInjectionAmount; count++)
                {
                    childContext.RegisterTemporal<InjectedClass>();
                }

                using (var childScope = childContext.Build())
                {
                    injectedClassList = childScope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();

                    Assert.That(injectedClassList.Count, Is.EqualTo(parentInjectionAmount + childInjectionAmount));
                }
            }
        }

        [Test]
        public void ShouldResolveEnumerable()
        {
            var parentInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < parentInjectionAmount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<IEnumerable<InjectedClass>>();

                Assert.That(injectedClassList.Count(), Is.EqualTo(parentInjectionAmount));

                var childInjectionAmount = new Random().Next(1, MaxMultipleInjectionCount);

                var childContext = scope.CreateContext();

                for (var count = 0; count < childInjectionAmount; count++)
                {
                    childContext.RegisterTemporal<InjectedClass>();
                }

                using (var childScope = childContext.Build())
                {
                    injectedClassList = childScope.Resolver.Resolve<IEnumerable<InjectedClass>>();

                    Assert.That(injectedClassList.Count(), Is.EqualTo(parentInjectionAmount + childInjectionAmount));
                }
            }
        }

        [Test]
        public void ShouldResolveLocalInstanceList()
        {
            var parentLocalInjectionCount = new Random().Next(1, MaxMultipleInjectionCount);

            var parentValue = 1;

            var context = new DependencyInjectionContext();

            for (var count = 0; count < parentLocalInjectionCount; count++)
            {
                context.RegisterLocal<InjectedStruct>().WithArgument("value", parentValue + count);
            }

            context.RegisterGlobal<InjectedStruct>().WithArgument("value", 0);

            using (var scope = context.Build())
            {
                var localInstanceList = scope.Resolver.Resolve<ILocalInstanceList<InjectedStruct>>();

                Assert.That(localInstanceList.InstanceList.Count, Is.EqualTo(parentLocalInjectionCount + 1));

                var childTemporalInjectionCount = new Random().Next(1, MaxMultipleInjectionCount);
                var childValue = parentLocalInjectionCount + 1;

                var childContext = scope.CreateContext();

                for (var count = 0; count < childTemporalInjectionCount; count++)
                {
                    childContext.RegisterTemporal<InjectedStruct>().WithArgument("value", childValue + count);
                }

                using (var childScope = childContext.Build())
                {
                    localInstanceList = childScope.Resolver.Resolve<ILocalInstanceList<InjectedStruct>>();

                    Assert.That(localInstanceList.InstanceList.Count, Is.EqualTo(parentLocalInjectionCount + childTemporalInjectionCount));
                }
            }
        }

        [Test]
        public void CannotInjectIntoFieldsWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>()
                .WithFieldInjection();

            Assert.That(() =>
            {
                _ = context.Build();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotInjectIntoPropertiesWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>()
                .WithPropertyInjection();

            Assert.That(() =>
            {
                _ = context.Build();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotInjectIntoMethodsWithoutDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>()
                .WithMethodInjection();

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

            using (var scope = context.Build())
            {
                var childContext = scope.CreateContext();
                childContext.RegisterTemporal<CircularDependencyClass3>();

                Assert.That(() =>
                {
                    _ = childContext.Build();

                }, Throws.TypeOf<Exception>());
            }
        }

        [Test]
        public void CannotResolveIfYouRegisterNothing()
        {
            var context = new DependencyInjectionContext();

            using (var scope = context.Build())
            {
                Assert.That(() =>
                {
                    _ = scope.Resolver.Resolve<InjectedClass>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    _ = scope.Resolver.Resolve<InjectedClass[]>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    _ = scope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    _ = scope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    _ = scope.Resolver.Resolve<IEnumerable<InjectedClass>>();
                }, Throws.TypeOf<Exception>());
            }
        }
    }
}
