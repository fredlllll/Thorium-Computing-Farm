namespace Thorium_Data
{
    public abstract class ADatabaseCommand
    {
        public bool IsQuery { get; set; } = false;

        //public abstract DbDataReader Execute();
    }
}
