using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FMTest.Business
{
    public class OrmSystem
    {
        internal IList<T> PopulateDataset<T>(string query, string connectionString, List<RdbmsParameter> queryParameters = null) where T : class
        {
            IList<T> result = new List<T>();

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (SQLiteConnection connection =
                new SQLiteConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SQLiteCommand command = new SQLiteCommand(query, connection);

                if (queryParameters != null)
                {
                    foreach (var parameter in queryParameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(parameter.Name, parameter.Value));
                    }
                }

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                connection.Open();
                using (command)
                {
                    var dataReader = command.ExecuteReader();

                    using (dataReader)
                    {
                        while (dataReader.Read())
                        {
                            var entry = Activator.CreateInstance<T>();

                            PropertyInfo[] props = typeof(T).GetProperties();
                            foreach (PropertyInfo prop in props)
                            {
                                object[] attrs = prop.GetCustomAttributes(true);

                                foreach (object attr in attrs)
                                {
                                    RdbmsNameAttribute rdbmsNameAttr = attr as RdbmsNameAttribute;

                                    if (rdbmsNameAttr != null)
                                    {
                                        string rawDbValue = null;

                                        try
                                        {
                                            rawDbValue = dataReader[rdbmsNameAttr._rdbmsName].ToString();
                                            var dbValClean = Convert.ChangeType(rawDbValue, prop.PropertyType);
                                            prop.SetValue(entry, dbValClean);
                                        }

                                        catch (Exception ex)
                                        {
                                            //Property data does not exist in query
                                        }
                                    }
                                }
                            }

                            result.Add(entry);
                        }
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Returns the Id of the last inserted row
        /// </summary>
        /// <param name="parameterizedQuery"></param>
        /// <param name="connectionString"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        internal int RunChangeQuery(string parameterizedQuery, string connectionString, List<RdbmsParameter> queryParameters = null)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SQLiteCommand command = new SQLiteCommand(parameterizedQuery, connection);

                if (queryParameters != null)
                {
                    foreach (var parameter in queryParameters)
                    {
                        command.Parameters.Add(new SQLiteParameter(parameter.Name, parameter.Value));
                    }
                }

                try
                {
                    using (command)
                    {
                        connection.Open();
                        var rowsAffected = command.ExecuteNonQuery();

                        return GetRowId(connection);
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private int GetRowId(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand("select last_insert_rowid() id;", connection);

            using (command)
            {
                var dataReader = command.ExecuteReader();

                using (dataReader)
                {
                    if (dataReader.Read())
                    {
                        return Convert.ToInt32(dataReader["id"]);
                    }

                    return 0;
                }
            }
        }
    }

    public class RdbmsParameter
    {
        public string Name { set; get; }
        public string Value { set; get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RdbmsNameAttribute : Attribute
    {
        public string _rdbmsName;

        public RdbmsNameAttribute(string propertyRdbmsName)
        {
            this._rdbmsName = propertyRdbmsName;
        }
    }


}