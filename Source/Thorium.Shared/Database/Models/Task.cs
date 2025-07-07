namespace Thorium.Shared.Database.Models
{
    public enum TaskStatus
    {
        Queued,
        Running,
        Finished
    }

    public class Task : Model
    {
        public required string JobId { get; set; }
        public required int TaskNumber { get; set; }
        public required TaskStatus Status { get; set; }
        public required bool WasSuccessful { get; set; }
        public string? LinedUpOnNodeId { get; set; }
    }
}
