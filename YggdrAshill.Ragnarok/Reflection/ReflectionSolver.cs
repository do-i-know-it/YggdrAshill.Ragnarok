using YggdrAshill.Ragnarok.Memorization;
using System;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    /// <summary>
    /// Implementation <see cref="ISolver"/> with Reflection.
    /// </summary>
    public sealed class ReflectionSolver :
        ISolver
    {
        public static ReflectionSolver Instance { get; } = new ReflectionSolver();

        private ReflectionSolver()
        {

        }

        public IActivation CreateActivation(ConstructorInjection injection)
        {
            return new ReflectionActivation(injection);
        }

        public IInfusion CreateFieldInfusion(FieldInjection injection)
        {
            return new ReflectionFieldInfusion(injection);
        }

        public IInfusion CreatePropertyInfusion(PropertyInjection injection)
        {
            return new ReflectionPropertyInfusion(injection);
        }

        public IInfusion CreateMethodInfusion(MethodInjection injection)
        {
            return new ReflectionMethodInfusion(injection);
        }

        public IActivation CreateCollectionActivation(Type elementType)
        {
            return new CollectionActivation(elementType);
        }
    }
}
