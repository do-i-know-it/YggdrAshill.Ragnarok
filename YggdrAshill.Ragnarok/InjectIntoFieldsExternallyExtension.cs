using YggdrAshill.Ragnarok.Fabrication;

namespace YggdrAshill.Ragnarok
{
    public static class InjectIntoFieldsExternallyExtension
    {
        public static IInjectIntoFieldsExternally From<T>(this IInjectIntoFieldsExternally injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.From(parameter);
        }
    }
}
