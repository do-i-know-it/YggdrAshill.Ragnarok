namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseNothing : IInfusion
    {
        public static InfuseNothing Instance { get; } = new();

        private InfuseNothing()
        {

        }

        public IDependency Dependency => WithoutDependency.Instance;

        public void Infuse(object instance, object[] parameterList)
        {

        }
    }
}
