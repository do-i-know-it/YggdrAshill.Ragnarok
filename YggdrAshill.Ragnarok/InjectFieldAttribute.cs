using System;

namespace YggdrAshill.Ragnarok
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class InjectFieldAttribute : Attribute
    {
    }
}
