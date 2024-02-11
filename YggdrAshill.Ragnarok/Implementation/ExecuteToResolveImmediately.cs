namespace YggdrAshill.Ragnarok
{
    internal sealed class ExecuteToResolveImmediately : IExecution
    {
        private readonly TypeAssignmentSource source;

        public ExecuteToResolveImmediately(TypeAssignmentSource source)
        {
            this.source = source;
        }

        public void Execute(IObjectResolver resolver)
        {
            var assignedTypeList = source.AssignedTypeList;

            var targetType = assignedTypeList.Count != 0 ? assignedTypeList[0] : source.ImplementedType;

            resolver.Resolve(targetType);
        }
    }
}
