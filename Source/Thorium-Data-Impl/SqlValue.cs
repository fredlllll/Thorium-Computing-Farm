namespace Thorium_Data_Impl
{
    public class SqlValue
    {
        public SqlValue(string column, object value)
        {
            Column = column;
            Value = value;
        }

        public string Column { get; set; }
        public object Value { get; set; }
    }
}
