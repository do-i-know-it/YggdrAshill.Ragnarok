using YggdrAshill.Ragnarok.Administration;
using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    public interface ITerminationCollection
    {
        void Collect(ITermination termination);
    }
}
