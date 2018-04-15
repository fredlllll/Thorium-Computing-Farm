namespace Thorium_Data.DatabaseCommands
{
    public class SelectCommand : ADatabaseCommand
    {
        public string[] Columns { get; set; }
        public string Table { get; set; }
        public DatabaseCondition[] Conditions { get; set; }
    }
}
