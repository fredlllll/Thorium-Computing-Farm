namespace Thorium_Data
{
    public class DatabaseCollation
    {
        public string Name { get; }

        public DatabaseCollation(string name)
        {
            Name = name;
        }

        public static DatabaseCollation UTF8MB4_Unicode_CI
        {
            get
            {
                return new DatabaseCollation("utf8mb4_unicode_ci");
            }
        }
    }
}
