using System.Reflection;

namespace YggdrAshill.Ragnarok
{
    public interface IDependencySelection
    {
        bool IsValid(ConstructorInfo info);
        bool IsValid(FieldInfo info);
        bool IsValid(PropertyInfo info);
        bool IsValid(MethodInfo info);
    }
}
