namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IParameterPropertyInjection : IMethodInjection
    {
        IParameterPropertyInjection WithProperty(IParameter parameter);
    }
}
