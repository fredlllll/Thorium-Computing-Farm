namespace Thorium_Data
{
    public class TableDefinition
    {
        public string Name { get; set; }
        public ColumnDefinition[] Columns { get; set; }
        public KeyDefinition[] KeyDefinitions { get; set; }
    }
}
