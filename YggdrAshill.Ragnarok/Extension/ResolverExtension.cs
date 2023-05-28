namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ResolverExtension
    {
        public static T Resolve<T>(this IResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T));
        }
    }
}
