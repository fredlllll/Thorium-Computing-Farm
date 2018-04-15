namespace Thorium_Data
{
    public enum ConditionComparison
    {
        Equals,
        Greater,
        GreaterEquals,
        Less,
        LessEquals,
        Like
    }

    public class DatabaseCondition
    {
        public bool Negate { get; set; }
        public string Column { get; set; }
        public ConditionComparison Comparison { get; set; }
        public object Value { get; set; }
    }
}
