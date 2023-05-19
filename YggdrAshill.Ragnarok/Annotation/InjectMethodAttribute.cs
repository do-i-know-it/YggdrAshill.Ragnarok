using System;

namespace YggdrAshill.Ragnarok
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InjectMethodAttribute : Attribute
    {
    }
}
