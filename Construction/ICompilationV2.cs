namespace YggdrAshill.Ragnarok
{
    public interface ICompilationV2
    {
        IInstantiation CreateInstantiation(DependencyInjectionRequest request);
        IInjection CreateFieldInjection(DependencyInjectionRequest request);
        IInjection CreatePropertyInjection(DependencyInjectionRequest request);
        IInjection CreateMethodInjection(DependencyInjectionRequest request);
    }
}
