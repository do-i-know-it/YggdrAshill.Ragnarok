namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoField :
        IAssignAnyType
    {
        IInjectIntoField With<T>(string name, T instance) where T : notnull;
        IInjectIntoProperty WithPropertyInjection();
        IInjectIntoMethod WithMethodInjection();
    }
}
