namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    // TODO: rename class?
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
