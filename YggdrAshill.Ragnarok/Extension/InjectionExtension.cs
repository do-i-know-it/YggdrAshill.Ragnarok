namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class InjectionExtension
    {
        public static IInstantiation ToInstantiate(this IInjection injection, IInstantiation instantiation)
        {
            return new InstantiateThenInject(instantiation, injection);
        }
    }
}
