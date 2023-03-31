namespace YggdrAshill.Ragnarok.Fabrication
{
    public interface IInjectIntoConstructor :
        IInjectIntoInstance
    {
        IInjectIntoConstructor With<T>(string name, T instance) where T : notnull;
    }
}
