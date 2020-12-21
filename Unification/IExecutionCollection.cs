using YggdrAshill.Ragnarok.Periodization;
using YggdrAshill.Ragnarok.Progression;

namespace YggdrAshill.Ragnarok.Unification
{
    public interface IExecutionCollection
    {
        ITermination Bind(IExecution execution);
    }
}
