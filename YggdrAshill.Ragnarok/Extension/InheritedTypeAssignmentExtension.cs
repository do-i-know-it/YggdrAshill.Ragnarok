using System.Runtime.CompilerServices;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class InheritedTypeAssignmentExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IInheritedTypeAssignment As<T>(this IInheritedTypeAssignment assignment)
            where T : notnull
        {
            return assignment.As(typeof(T));
        }
    }
}
