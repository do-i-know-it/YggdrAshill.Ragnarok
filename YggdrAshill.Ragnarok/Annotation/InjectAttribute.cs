using System;

namespace YggdrAshill.Ragnarok
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
    }
}
