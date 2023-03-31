namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class CircularDependencyClass1
    {
        private readonly CircularDependencyClass2 circularDependencyClass2;

        [Inject]
        public CircularDependencyClass1(CircularDependencyClass2 circularDependencyClass2)
        {
            this.circularDependencyClass2 = circularDependencyClass2;
        }
    }
}
