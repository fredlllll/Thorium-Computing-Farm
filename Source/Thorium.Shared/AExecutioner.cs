namespace Thorium.Shared
{
    public abstract class AExecutioner
    {
        public LightweightTask Task { get; protected set; }

        public AExecutioner(LightweightTask t) {
            Task = t;
        }

        public abstract ExecutionResult Execute();
    }
}
