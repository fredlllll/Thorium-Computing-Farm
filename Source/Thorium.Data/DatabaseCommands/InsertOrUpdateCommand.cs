namespace Thorium_Data.DatabaseCommands
{
    public class InsertOrUpdateCommand : ADatabaseCommand
    {
        public string Table { get; set; }
        public string[] Columns { get; set; }
        public object[] Values { get; set; }
    }
}
