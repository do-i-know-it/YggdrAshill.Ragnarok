using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Unification
{
    public interface IExecutionCollection
    {
        ITermination Collect(IExecution execution);
    }
}
