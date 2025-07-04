namespace Thorium.Shared.Messages
{
    public class FunctionCall
    {
        public int Id { get; set; }
        public string FunctionName { get; set; }
        public object[] FunctionArguments { get; set; }
        public bool NeedsAnwer { get; set; }
    }
}
