namespace Thorium.Shared.DTOs.OperationData
{
    public class ExeDTO
    {
        public string FilePath {  get; set; }
        public string[] Arguments { get; set; }
        public string WorkingDir {  get; set; }
    }
}
