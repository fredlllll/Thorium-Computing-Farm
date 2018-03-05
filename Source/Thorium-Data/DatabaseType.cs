namespace Thorium_Data
{
    /// <summary>
    /// derive from this type to create your database types
    /// </summary>
    public abstract class DatabaseType
    {
        private readonly string type;

        public DatabaseType(string type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            return type;
        }
    }
}
