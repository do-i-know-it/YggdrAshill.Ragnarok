using YggdrAshill.Ragnarok.Construction;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    public interface IRegistryBuilder :
        ICodeBuilder
    {
        IRegistry Build(IEnumerable<IDescription> descriptionList);
    }
}
