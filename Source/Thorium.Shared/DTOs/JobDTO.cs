namespace Thorium.Shared.DTOs
{
    public class JobDTO
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public required int TaskCount { get; set; }

        public required OperationDTO[] Operations { get; set; }
        //TODO: all the other things
    }
}
