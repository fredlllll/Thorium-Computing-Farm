namespace Thorium.Shared.Database.Models
{
    public class Node :Model
    {
        public required string Identifier { get; set; }
        public Task? Task { get; set; }
    }
}
