namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoInstance :
        IAssignAnyType
    {
        IInjectIntoField WithFieldInjection();
        IInjectIntoProperty WithPropertyInjection();
        IInjectIntoMethod WithMethodInjection();
    }
}
