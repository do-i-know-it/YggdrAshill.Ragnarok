using YggdrAshill.Ragnarok.Administration;

namespace YggdrAshill.Ragnarok
{
    public interface ITerminationCollection
    {
        void Collect(ITermination termination);
    }
}
