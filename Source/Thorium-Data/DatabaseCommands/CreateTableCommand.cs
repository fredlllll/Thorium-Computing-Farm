namespace Thorium_Data.DatabaseCommands
{
    public class CreateTableCommand : ADatabaseCommand
    {
        public TableDefinition TableDefinition { get; }
        public CreateTableCommand(TableDefinition tableDefinition)
        {
            TableDefinition = tableDefinition;
        }
    }
}
