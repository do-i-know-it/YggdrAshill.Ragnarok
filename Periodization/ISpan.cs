namespace YggdrAshill.Ragnarok.Periodization
{
    /// <summary>
    /// Scope or term from <see cref="IOrigination"/> to <see cref="ITermination"/>.
    /// </summary>
    public interface ISpan :
        IOrigination,
        ITermination
    {

    }
}
