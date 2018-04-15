namespace Thorium_Data
{
    public class ColumnDefinition
    {
        public string Name { get; set; }
        public DatabaseType Type { get; set; } //TODO: how do i do length?
        public object LengthOrValues { get; set; }
        public object StandardValue { get; set; }
        public DatabaseCollation Collation { get; set; }
        public ColumnAttribute Attribute { get; set; }
        public bool Null { get; set; }
        public bool AutoIncrement { get; set; }
        public string Comment { get; set; }
    }
}
