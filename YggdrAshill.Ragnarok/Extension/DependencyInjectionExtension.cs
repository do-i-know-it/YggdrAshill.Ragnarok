namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class DependencyInjectionExtension
    {
        public static IDependencyInjection WithArgument<T>(this IDependencyInjection injection, string name, T instance)
            where T : notnull
        {
            var parameter = new Parameter<T>(name, instance);

            return injection.WithArgument(parameter);
        }
    }
}
