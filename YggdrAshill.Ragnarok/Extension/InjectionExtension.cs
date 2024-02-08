using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class InjectionExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IInstantiation ToInstantiate(this IInjection injection, IInstantiation instantiation)
        {
            return new InstantiateThenInject(instantiation, injection);
        }
    }
}
