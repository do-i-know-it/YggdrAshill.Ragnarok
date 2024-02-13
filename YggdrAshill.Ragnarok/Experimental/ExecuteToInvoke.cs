namespace YggdrAshill.Ragnarok
{
    internal sealed class ExecuteToInvoke<T> : IExecution
        where T : notnull
    {
        private readonly IInvocation<T> invocation;

        public ExecuteToInvoke(IInvocation<T> invocation)
        {
            this.invocation = invocation;
        }

        public void Execute(IObjectResolver resolver)
        {
            var instance = resolver.Resolve<T>();

            invocation.Invoke(instance);
        }
    }
}
