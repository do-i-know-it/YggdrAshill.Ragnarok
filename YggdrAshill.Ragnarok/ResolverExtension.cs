using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok
{
    public static class ResolverExtension
    {
        public static T Resolve<T>(this IResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T));
        }
    }
}
