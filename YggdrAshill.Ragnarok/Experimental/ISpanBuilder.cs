using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok.Experimental
{
    internal interface ISpanBuilder
    {
        ISpanBuilder Configure(ISpan span);

        ISpan Build();
    }
}
