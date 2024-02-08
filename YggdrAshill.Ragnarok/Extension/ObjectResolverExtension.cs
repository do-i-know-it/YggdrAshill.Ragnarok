namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class ObjectResolverExtension
    {
        public static T Resolve<T>(this IObjectResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T));
        }
    }
}
