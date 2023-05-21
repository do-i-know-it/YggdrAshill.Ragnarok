using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnInstanceDirectly :
        IInstantiation
    {
        private readonly object instance;

        public ReturnInstanceDirectly(object instance)
        {
            this.instance = instance;
        }

        public object Instantiate(IResolver resolver)
        {
            return instance;
        }
    }
}
