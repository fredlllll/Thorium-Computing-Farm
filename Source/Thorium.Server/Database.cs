using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Thorium.Server
{
    public class Database
    {
        static SQLiteConnection connection;

        static Database()
        {
            connection = new SQLiteConnection("Data Source=database.db");
            connection.Open();
        }

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
    }
}
