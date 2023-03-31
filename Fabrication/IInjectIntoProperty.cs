namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoProperty :
        IAssignAnyType
    {
        IInjectIntoProperty With<T>(string name, T instance) where T : notnull;
        IInjectIntoMethod WithMethodInjection();
    }
}
