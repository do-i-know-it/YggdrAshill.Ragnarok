using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok.Materialization
{
    public sealed class PropertyInjection
    {
        private readonly PropertyInfo[] propertyList;
        public PropertyInfo[] PropertyList => propertyList.ToArray();

        public PropertyInjection(PropertyInfo[] propertyList)
        {
            this.propertyList = propertyList;
        }
    }
}
