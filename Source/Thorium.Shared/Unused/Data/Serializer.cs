namespace Thorium.Shared.Data
{
    /*public class Serializer
    {
        IDatabase database;

        public Serializer(IDatabase db)
        {
            database = db;
        }

        void CheckTypeIsDatabaseObject(Type t)
        {
            if(!typeof(DatabaseObject).IsAssignableFrom(t))
            {
                throw new ArgumentException("The type has to be a descendant of " + nameof(DatabaseObject));
            }
        }

        string GetTableName(Type t)
        {
            return t.FullName.Replace('.', '_');
        }

        IEnumerable<PropertyInfo> GetDataProperties(Type t)
        {
            int primaryKeys = 0;
            var allProperties = t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<PropertyInfo> filteredProperties = new List<PropertyInfo>();
            foreach(var prop in allProperties)
            {
                if(prop.SetMethod != null && Attribute.IsDefined(prop, typeof(DataMemberAttribute)))
                {
                    if(Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute)))
                    {
                        primaryKeys++;
                    }
                    filteredProperties.Add(prop);
                }
            }
            if(primaryKeys != 1)
            {
                throw new ArgumentException("Type must have exactly 1 primary key");
            }
            return filteredProperties;
        }

        void CreateTable(Type t)
        {
            CheckTypeIsDatabaseObject(t);

            string tableName = GetTableName(t);

            string sql = "CREATE TABLE IF NOT EXISTS " + tableName + "(";
            foreach(var prop in GetDataProperties(t))
            {
                sql += prop.Name;
                sql += " ";
                //sql += Util.TypeAffinityMap[prop.PropertyType]; //TODO: commented due to code changes
                if(Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute)))
                {
                    sql += " PRIMARY KEY";
                    if(Attribute.IsDefined(prop, typeof(AutoIncrementAttribute)))
                    {
                        sql += " AUTOINCREMENT";
                    }
                }
                sql += ",";
            }
            sql = sql.TrimEnd(',');
            sql += ")";
            database.ExecuteNonQueryTransaction(sql);
        }

        DatabaseObject GetInstance(Type t)
        {
            var ci = t.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            if(ci == null)
            {
                return (DatabaseObject)FormatterServices.GetUninitializedObject(t);
            }
            return (DatabaseObject)ci.Invoke(null);
        }

        public long Save(DatabaseObject obj)
        {
            Type t = obj.GetType();
            CreateTable(t);
            string tableName = GetTableName(t);

            var properties = GetDataProperties(t);
            var idProp = properties.First((p) => p.Name == nameof(DatabaseObject.___Id___));
            properties = properties.Except(new PropertyInfo[] { idProp }); //idProp has to be processed seperately

            Dictionary<string, object> propertyValues = new Dictionary<string, object>();

            long id = Convert.ToInt64(idProp.GetValue(obj));
            if(id >= 0)
            {
                propertyValues[idProp.Name] = id;
            }
            foreach(var prop in properties)
            {
                propertyValues[prop.Name] = prop.GetValue(obj);
            }

            //TODO: check if this also works with normal sql
            string sql = "INSERT OR REPLACE INTO " + tableName + "(";

            foreach(var kv in propertyValues)
            {
                sql += kv.Key + ",";
            }

            sql = sql.TrimEnd(',');
            sql += ") VALUES(";

            for(int i = 0; i < propertyValues.Count; i++)
            {
                sql += "@" + i + ",";
            }
            sql = sql.TrimEnd(',');
            sql += ");";

            //long newId = database.ExecuteNonQueryTransaction(sql, propertyValues.Values.ToArray());
            //commented cause of code changes
            long newId = -1;//TODO: find better way than relying on sqlites implementation to give back last insert id

            if(id < 0)
            { //only set id if it was -1 before
                idProp.SetValue(obj, newId);
            }
            return id;
        }

        public Dictionary<long, DatabaseObject> LoadObjects(Type t)
        {
            CreateTable(t);
            string tableName = GetTableName(t);
            var properties = GetDataProperties(t);
            Dictionary<long, DatabaseObject> objects = new Dictionary<long, DatabaseObject>();

            string sql = "SELECT * FROM " + tableName;

            using(var reader = database.ExecuteQuery(sql))
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        DatabaseObject instance = GetInstance(t);
                        foreach(var prop in properties)
                        {
                            var tmp = Convert.ChangeType(reader[prop.Name], prop.PropertyType);
                            prop.SetValue(instance, tmp);
                        }
                        instance.Serializer = this;
                        objects[(long)reader[nameof(DatabaseObject.___Id___)]] = instance;
                    }
                }
            }
            return objects;
        }

        public DatabaseObject LoadObject(Type t, long id)
        {
            CreateTable(t);
            string tableName = GetTableName(t);
            var properties = GetDataProperties(t);
            string sql = "SELECT * FROM " + tableName + " WHERE " + nameof(DatabaseObject.___Id___) + " = " + id;

            using(var reader = database.ExecuteQuery(sql))
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    DatabaseObject instance = GetInstance(t);
                    foreach(var prop in properties)
                    {
                        prop.SetValue(instance, reader[prop.Name]);
                    }
                    instance.Serializer = this;
                    return instance;
                }
            }
            return null;
        }

        public void DeleteObject(DatabaseObject obj)
        {
            Type t = obj.GetType();
            CreateTable(t);

            string tableName = GetTableName(t);

            string sql = "DELETE FROM " + tableName + " WHERE " + nameof(DatabaseObject.___Id___) + " = " + obj.___Id___;
            database.ExecuteNonQueryTransaction(sql);
        }

        public void DeleteObjects(Type t)
        {
            string tableName = GetTableName(t);
            string sql = "TRUNCATE TABLE " + tableName;
            database.ExecuteNonQueryTransaction(sql);
        }

        public void DoMigration(Type t)
        {
            //TODO: compare table structure with structure of type, and alter table structure if difference is found
            throw new NotImplementedException();
        }
    }*/
}
