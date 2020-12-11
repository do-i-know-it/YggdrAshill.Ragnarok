using YggdrAshill.Ragnarok.Administration;

namespace YggdrAshill.Ragnarok
{
    public interface IExecutionCollection
    {
        void Collect(IExecution execution);
    }
}
