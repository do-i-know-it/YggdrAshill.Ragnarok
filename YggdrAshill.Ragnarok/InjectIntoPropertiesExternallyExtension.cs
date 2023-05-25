using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok
{
    public static class InjectIntoPropertiesExternallyExtension
    {
        public static IInjectIntoPropertiesExternally From<T>(this IInjectIntoPropertiesExternally injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.From(parameter);
        }
    }
}
