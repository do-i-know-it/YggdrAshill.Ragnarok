using System.Linq;
using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    public sealed class FieldInjection
    {
        private readonly FieldInfo[] fieldList;
        public FieldInfo[] FieldList => fieldList.ToArray();

        public FieldInjection(FieldInfo[] fieldList)
        {
            this.fieldList = fieldList;
        }
    }
}
