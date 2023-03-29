namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoMethod :
        IAssignAnyType
    {
        IInjectIntoMethod With<T>(string name, T instance) where T : notnull;
    }
}
