using System;

namespace YggdrAshill.Ragnarok
{
    internal static class TypeValidation
    {
        public static bool CanInstantiate(Type type)
        {
            return !IsStatic(type) && !IsPrimitive(type) && !type.IsInterface && !type.IsAbstract && (type.IsClass || type.IsValueType);
        }

        private static bool IsStatic(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        private static bool IsPrimitive(Type type)
        {
            // https://learn.microsoft.com/ja-jp/dotnet/api/system.type.isprimitive?view=net-8.0#system-type-isprimitive
            // Primitives are Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, Single.
            // decimal, object, string are not primitives.
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal);
        }
    }
}
