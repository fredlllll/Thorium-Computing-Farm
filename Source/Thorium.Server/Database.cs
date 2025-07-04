//using System.Data.SQLite;
namespace Thorium.Server
{
    /*
    public class Database
    {
        static SQLiteConnection connection;

        static Database()
        {
            bool needsInit = !File.Exists("database.db");
            connection = new SQLiteConnection("Data Source=database.db");
            connection.Open();
            if (needsInit)
            {
                InitDb();
            }
        }

        static void InitDb()
        {
            CreateTable("jobs", "id", "name", "description");
            CreateTable("tasks", "id", "job_id", "index", "status");
            CreateTable("operations", "id", "job_id", "index", "type");
            CreateTable("op_exes", "op_id", "file_path", "arguments", "working_dir");
        }

        public static void CreateTable(string tableName, params string[] fieldNames)
        {
            var sql = $"CREATE TABLE IF NOT EXISTS `{tableName}`(";
            sql += string.Join(',', fieldNames.Select(x=>"`"+x+"`"));
            sql += ")";
            ExecuteNonQuery(sql);
        }

        public static void ExecuteNonQuery(string sql)
        {
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static void ExecuteNonQuery(string sql, params object[] parameters)
        {
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = sql;
                foreach (var val in parameters)
                {
                    SQLiteParameter parameter = cmd.CreateParameter();
                    parameter.Value = val;
                    cmd.Parameters.Add(parameter);
                }
                cmd.ExecuteNonQuery();
            }
        }

        public static IEnumerable<SQLiteDataReader> ExecuteQuery(string sql, params object[] parameters)
        {
            using (var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = sql;
                foreach (var val in parameters)
                {
                    SQLiteParameter parameter = cmd.CreateParameter();
                    parameter.Value = val;
                    cmd.Parameters.Add(parameter);
                }
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader;
                    }
                }
            }
        }

        #region old stuff

        private static string GetTableName(Type type)
        {
            return type.FullName.Replace('.', '_');
        }

        private static IEnumerable<PropertyInfo> GetRelevantProperties(Type type)
        {
            return type.GetProperties().Where(x => x.CanRead && x.CanWrite);
        }

        private static PropertyInfo[] GetRelevantPropertiesArray(Type type)
        {
            return GetRelevantProperties(type).ToArray();
        }

        private static IEnumerable<string> GetPropertyNames(Type type)
        {
            return GetRelevantProperties(type).Select(x => x.Name);
        }

        private static string GetPropertyNamesList(Type type)
        {
            return string.Join(',', GetPropertyNames(type));
        }

        private static string GetId(object obj)
        {
            throw new NotImplementedException("only support models so far");
        }

        private static string GetReference(object obj)
        {
            return "##REF##" + obj.GetType().FullName + "#" + GetId(obj);
        }

        public static void CreateTable<T>()
        {
            Type type = typeof(T);

            var tableName = GetTableName(type);

            using (var cmd = new SQLiteCommand(connection))
            {
                var text = $"CREATE TABLE IF NOT EXISTS `{tableName}`(";
                text += GetPropertyNamesList(type);
                text += ")";
                cmd.CommandText = text;
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertOne(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Can't insert null values");
            }
            Type type = entity.GetType();
            var props = GetRelevantProperties(type);

            var tableName = GetTableName(type);

            using (SQLiteTransaction mytransaction = connection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(connection))
                {
                    var text = $"INSERT INTO {tableName} (";
                    text += GetPropertyNamesList(type);
                    text += ") VALUES (" + string.Join(',', '?') + ")";
                    mycommand.CommandText = text;

                    foreach (var property in props)
                    {
                        SQLiteParameter parameter = mycommand.CreateParameter();
                        var value = property.GetValue(entity);
                        parameter.Value = value;
                        if (parameter.DbType == System.Data.DbType.Object)
                        {
                            InsertOne(value);
                            parameter.Value = GetReference(value);
                        }
                        mycommand.Parameters.Add(parameter);
                    }

                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
        }

        public static void InsertMany<T>(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            Type type = typeof(T);
            var props = GetRelevantPropertiesArray(type);

            var tableName = GetTableName(type);

            using (SQLiteTransaction mytransaction = connection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(connection))
                {
                    var text = $"INSERT INTO {tableName} (";
                    text += GetPropertyNamesList(type);
                    text += ") VALUES (" + string.Join(',', '?') + ")";
                    mycommand.CommandText = text;

                    var parameters = new SQLiteParameter[props.Length];
                    for (int i = 0; i < props.Length; i++)
                    {
                        SQLiteParameter parameter = mycommand.CreateParameter();
                        parameters[i] = parameter;
                        mycommand.Parameters.Add(parameter);
                    }

                    foreach (var entity in entities)
                    {
                        for (int i = 0; i < props.Length; i++)
                        {
                            var property = props[i];
                            var parameter = parameters[i];
                            var value = property.GetValue(entity);
                            parameter.Value = value;
                            if (parameter.DbType == System.Data.DbType.Object)
                            {
                                InsertOne(value);
                                parameter.Value = GetReference(value);
                            }
                        }
                        mycommand.ExecuteNonQuery();
                    }
                }
                mytransaction.Commit();
            }
        }

        public static object SqlToObject(Type type, string sql)
        {
            using (SQLiteCommand mycommand = new SQLiteCommand(connection))
            {
                mycommand.CommandText = sql;
                using (var reader = mycommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var value = RuntimeHelpers.GetUninitializedObject(type);
                        var props = GetRelevantProperties(type);
                        foreach (var property in props)
                        {
                            property.SetValue(value, reader[property.Name]);
                        }
                        return value;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<object> SqlToObjects(Type type, string sql)
        {
            var props = GetRelevantProperties(type); //while its not cached, get it here so its not queried multiple times

            using (SQLiteCommand mycommand = new SQLiteCommand(connection))
            {
                mycommand.CommandText = sql;
                using (var reader = mycommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var value = RuntimeHelpers.GetUninitializedObject(type);

                        foreach (var property in props)
                        {
                            property.SetValue(value, reader[property.Name]);
                        }
                        yield return value;
                    }
                }
            }
        }

        public static object FindOneByQuery(Type type, string query)
        {
            var tableName = GetTableName(type);
            var sql = $"SELECT {GetPropertyNamesList(type)} FROM {tableName} {query} LIMIT 1";
            return SqlToObject(type, sql);
        }

        public static IEnumerable<object> FindManyByQuery(Type type, string query)
        {
            var tableName = GetTableName(type);
            var sql = $"SELECT {GetPropertyNamesList(type)} FROM {tableName} {query}";
            return SqlToObjects(type, sql);
        }

        #endregion
    }
    */
}
