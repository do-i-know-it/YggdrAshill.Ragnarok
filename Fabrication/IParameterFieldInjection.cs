namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public interface IParameterFieldInjection : IPropertyInjection
    {
        IParameterFieldInjection WithField(IParameter parameter);
    }
}
