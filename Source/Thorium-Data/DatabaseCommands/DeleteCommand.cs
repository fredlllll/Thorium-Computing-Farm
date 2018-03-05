namespace Thorium_Data.DatabaseCommands
{
    public class DeleteCommand : ADatabaseCommand
    {
        public string Table { get; set; }
        public DatabaseCondition[] Conditions { get; set; }
    }
}
