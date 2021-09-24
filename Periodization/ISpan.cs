namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Lifetime from <see cref="IOrigination"/> to <see cref="ITermination"/>.
    /// </summary>
    public interface ISpan :
        IOrigination,
        ITermination
    {

    }
}
