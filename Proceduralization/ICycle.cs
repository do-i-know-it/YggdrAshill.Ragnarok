using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Proceduralization
{
    /// <summary>
    /// Lifecycle for <see cref="IExecution"/> between <see cref="IOrigination"/> and <see cref="ITermination"/>.
    /// </summary>
    public interface ICycle :
        IOrigination,
        ITermination,
        IExecution
    {

    }
}
