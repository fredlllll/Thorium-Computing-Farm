namespace Thorium.Shared.Database
{
    public class DatabaseConnection
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string Database { get; set; }
        public required string User { get; set; }
        public required string Password { get; set; }

        /*public NpgsqlConnection? Connection { get; set; }

        public void Connect()
        {
            string connectionString = GetConnectionString();
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }*/

        public string GetConnectionString()
        {
            string connectionString = $"Host={Host};Port={Port};Database={Database};User ID={User};";
            if (Password.Length > 0)
            {
                connectionString += "Password=" + Password + ";";
            }
            return connectionString;
        }

        public static DatabaseConnection LoadFromSettings()
        {
            var connection = new DatabaseConnection
            {
                Host = Settings.Get<string>("postgresHost", null) ?? "127.0.0.1",
                Port = Settings.Get<int>("postgresPort", 5432),
                Database = Settings.Get<string>("postgresDatabase", null) ?? "thorium",
                User = Settings.Get<string>("postgresUser", null) ?? "postgres",
                Password = Settings.Get<string>("postgresPassword", null) ?? "postgres"
            };
            return connection;
        }
    }
}
