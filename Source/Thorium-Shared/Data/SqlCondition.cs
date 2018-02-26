namespace Thorium_Shared.Data
{
    public class SqlCondition : SqlValue
    {
        public SqlCondition(string column, object shouldBe) : base(column, shouldBe)
        {

        }

        public object ShouldBe { get { return Value; } set { Value = value; } }
    }
}
