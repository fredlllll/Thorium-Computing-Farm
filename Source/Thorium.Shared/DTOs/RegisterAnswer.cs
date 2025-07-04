namespace Thorium.Shared.DTOs
{
    public class RegisterAnswer
    {
        public required string NodeId { get; set; }
        public required string DatabaseHost { get; set; }
        public required int DatabasePort { get; set; }
        public required string DatabaseUser { get; set; }
        public required string DatabasePassword { get; set; }
        public required string DatabaseName { get; set; }
    }
}
