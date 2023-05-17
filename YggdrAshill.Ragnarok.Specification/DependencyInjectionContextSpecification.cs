using YggdrAshill.Ragnarok.Construction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok.Specification
{
    [TestFixture(TestOf = typeof(DependencyInjectionContext))]
    internal sealed class DependencyInjectionContextSpecification
    {
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

            context.RegisterGlobal<InjectedClass>();

            using (var scope = context.Build())
            {
                var instance1 = scope.Resolver.Resolve<InjectedClass>();
                var instance2 = scope.Resolver.Resolve<InjectedClass>();

                Assert.That(instance1, Is.EqualTo(instance2));

                using (var child = scope.CreateScope())
                {
                    var instance3 = child.Resolver.Resolve<InjectedClass>();

                    Assert.That(instance1, Is.EqualTo(instance3));
                    Assert.That(instance2, Is.EqualTo(instance3));
                }
            }
        }

        [Test]
        public void ShouldInjectConstantValueIntoConstructor()
        {
            var value = new Random().Next();

            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedStruct>()
                .With("value", value);

            using (var scope = context.Build())
            {
                var instance = scope.Resolver.Resolve<InjectedStruct>();

                Assert.That(instance.Value, Is.EqualTo(value));
            }
        }

        [Test]
        public void ShouldInjectDependenciesIntoConstructor()
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<InjectedClass>();
            context.RegisterLocal<InjectedStruct>()
                .With("value", new Random().Next());
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
        public void ShouldEnableFieldInjectionIfYouWantToInjectDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<InjectedClass>();
            context.RegisterLocal<InjectedStruct>()
                .With("value", new Random().Next());
            context.RegisterTemporal<FieldInjectableClass>()
                .WithFieldInjection();

            using (var scope = context.Build())
            {
                var injectedClass = scope.Resolver.Resolve<InjectedClass>();
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<FieldInjectableClass>();

                Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
                Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
            }
        }

        [Test]
        public void ShouldEnablePropertyInjectionIfYouWantToInjectDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<InjectedClass>();
            context.RegisterLocal<InjectedStruct>()
                .With("value", new Random().Next());
            context.RegisterTemporal<PropertyInjectableClass>()
                .WithPropertyInjection();

            using (var scope = context.Build())
            {
                var injectedClass = scope.Resolver.Resolve<InjectedClass>();
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<PropertyInjectableClass>();

                Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
                Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
            }
        }

        [Test]
        public void ShouldEnableMethodInjectionIfYouWantToInjectDependencies()
        {
            var context = new DependencyInjectionContext();

            context.RegisterLocal<InjectedClass>();
            context.RegisterLocal<InjectedStruct>()
                .With("value", new Random().Next());
            context.RegisterTemporal<MethodInjectableClass>()
                .WithMethodInjection();

            using (var scope = context.Build())
            {
                var injectedClass = scope.Resolver.Resolve<InjectedClass>();
                var injectedStruct = scope.Resolver.Resolve<InjectedStruct>();
                var instance = scope.Resolver.Resolve<MethodInjectableClass>();

                Assert.That(instance.InjectedClass, Is.EqualTo(injectedClass));
                Assert.That(instance.InjectedStruct, Is.EqualTo(injectedStruct));
            }
        }

        [Test]
        public void ShouldResolveArrayInSameScope()
        {
            var amount = new Random().Next(1, 10);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < amount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<InjectedClass[]>();

                Assert.That(injectedClassList.Count, Is.EqualTo(amount));
            }
        }

        [Test]
        public void ShouldResolveReadOnlyListInSameScope()
        {
            var amount = new Random().Next(1, 10);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < amount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();

                Assert.That(injectedClassList.Count, Is.EqualTo(amount));
            }
        }

        [Test]
        public void ShouldResolveReadOnlyCollectionInSameScope()
        {
            var amount = new Random().Next(1, 10);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < amount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();

                Assert.That(injectedClassList.Count, Is.EqualTo(amount));
            }
        }

        [Test]
        public void ShouldResolveEnumerableInSameScope()
        {
            var amount = new Random().Next(1, 10);

            var context = new DependencyInjectionContext();

            for (var count = 0; count < amount; count++)
            {
                context.RegisterTemporal<InjectedClass>();
            }

            using (var scope = context.Build())
            {
                var injectedClassList = scope.Resolver.Resolve<IEnumerable<InjectedClass>>();

                Assert.That(injectedClassList.Count(), Is.EqualTo(amount));
            }
        }

        [Test]
        public void ShouldResolveArrayInAllScope()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>();

            using (var scope = context.Build())
            {
                var childContext = scope.CreateContext();

                childContext.RegisterTemporal<InjectedClass>();

                using (var childScope = childContext.Build())
                {
                    var injectedClassList = childScope.Resolver.Resolve<InjectedClass[]>();

                    Assert.That(injectedClassList.Count(), Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void ShouldResolveReadOnlyListInAllScope()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>();

            using (var scope = context.Build())
            {
                var childContext = scope.CreateContext();

                childContext.RegisterTemporal<InjectedClass>();

                using (var childScope = childContext.Build())
                {
                    var injectedClassList = childScope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();

                    Assert.That(injectedClassList.Count(), Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void ShouldResolveReadOnlyCollectionInAllScope()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>();

            using (var scope = context.Build())
            {
                var childContext = scope.CreateContext();

                childContext.RegisterTemporal<InjectedClass>();

                using (var childScope = childContext.Build())
                {
                    var injectedClassList = childScope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();

                    Assert.That(injectedClassList.Count(), Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void ShouldResolveEnumerableInAllScope()
        {
            var context = new DependencyInjectionContext();

            context.RegisterTemporal<InjectedClass>();

            using (var scope = context.Build())
            {
                var childContext = scope.CreateContext();

                childContext.RegisterTemporal<InjectedClass>();

                using (var childScope = childContext.Build())
                {
                    var injectedClassList = childScope.Resolver.Resolve<IEnumerable<InjectedClass>>();

                    Assert.That(injectedClassList.Count(), Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void CannotEnableFieldInjectionIfHaveNoDependencies()
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
        public void CannotEnablePropertyInjectionIfHaveNoDependencies()
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
        public void CannotEnableMethodInjectionIfHaveNoDependencies()
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
            context.RegisterTemporal<CircularDependencyClass3>();

            Assert.That(() =>
            {
                _ = context.Build();
            }, Throws.TypeOf<Exception>());
        }

        [Test]
        public void CannotResolveIfYouRegisterNothing()
        {
            var context = new DependencyInjectionContext();

            using (var scope = context.Build())
            {
                Assert.That(() =>
                {
                    scope.Resolver.Resolve<InjectedClass>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    scope.Resolver.Resolve<InjectedClass[]>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    scope.Resolver.Resolve<IReadOnlyList<InjectedClass>>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    scope.Resolver.Resolve<IReadOnlyCollection<InjectedClass>>();
                }, Throws.TypeOf<Exception>());

                Assert.That(() =>
                {
                    scope.Resolver.Resolve<IEnumerable<InjectedClass>>();
                }, Throws.TypeOf<Exception>());
            }
        }
    }
}
