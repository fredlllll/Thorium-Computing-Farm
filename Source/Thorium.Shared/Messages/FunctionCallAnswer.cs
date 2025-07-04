namespace Thorium.Shared.Messages
{
    public class FunctionCallAnswer
    {
        public int Id { get; set; }
        public object ReturnValue { get; set; }
        public string Exception { get; set; }
    }
}
