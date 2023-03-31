namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IAssignAnyType
    {
        IAfterAnyTypeAssigned As<T>() where T : notnull;
        IAfterImplementedTypeAssigned AsSelf();
        IAfterImplementedInterfacesAssigned AsImplementedInterfaces();
    }
}
