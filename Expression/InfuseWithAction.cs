namespace YggdrAshill.Ragnarok
{
    internal sealed class InfuseWithAction : IInfusion
    {
        private readonly ActionToInfuse onInfused;

        public InfuseWithAction(ActionToInfuse onInfused)
        {
            this.onInfused = onInfused;
        }

        public void Infuse(ref object instance, object[] parameterList)
        {
            onInfused.Invoke(ref instance, parameterList);
        }
    }
}
