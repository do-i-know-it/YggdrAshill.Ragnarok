using YggdrAshill.Ragnarok.Periodization;

namespace YggdrAshill.Ragnarok
{
    internal interface ISpanBuilder
    {
        ISpanBuilder Configure(ISpan span);

        ISpan Build();
    }
}
