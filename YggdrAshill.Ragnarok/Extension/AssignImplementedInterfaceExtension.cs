namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class AssignImplementedInterfaceExtension
    {
        public static IAssignImplementedInterface As<T>(this IAssignImplementedInterface assignment)
            where T : notnull
        {
            return assignment.As(typeof(T));
        }
    }
}
