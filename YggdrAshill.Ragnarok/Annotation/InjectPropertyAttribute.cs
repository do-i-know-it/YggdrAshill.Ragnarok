using System;

namespace YggdrAshill.Ragnarok
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class InjectPropertyAttribute : Attribute
    {
    }
}
