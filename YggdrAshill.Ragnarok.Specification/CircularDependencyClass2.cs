namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class CircularDependencyClass2
    {
        private readonly CircularDependencyClass3 circularDependencyClass3;

        [Inject]
        public CircularDependencyClass2(CircularDependencyClass3 circularDependencyClass3)
        {
            this.circularDependencyClass3 = circularDependencyClass3;
        }
    }
}
