namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseNothing : IInfusion
    {
        public static InfuseNothing Instance { get; } = new();

        private InfuseNothing()
        {

        }

        public void Infuse(object instance, object[] parameterList)
        {

        }
    }
}
