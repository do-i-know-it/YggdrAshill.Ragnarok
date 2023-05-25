using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok
{
    public static class InjectIntoMethodExternallyExtension
    {
        public static IInjectIntoMethodExternally From<T>(this IInjectIntoMethodExternally injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.From(parameter);
        }
    }
}
