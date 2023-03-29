namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class CircularDependencyClass3
    {
        private readonly CircularDependencyClass1 circularDependencyClass1;

        [Inject]
        public CircularDependencyClass3(CircularDependencyClass1 circularDependencyClass1)
        {
            this.circularDependencyClass1 = circularDependencyClass1;
        }
    }
}
