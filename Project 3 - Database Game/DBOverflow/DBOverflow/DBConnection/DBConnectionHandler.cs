using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace DBOverflow.DBConnection
{
    class DBConnectionHandler
    {
        private string serverName;
        private string username;
        private string database;
        private NpgsqlConnection dbconnection;

        public DBConnectionHandler(string server, string user, string db)
        {
            this.serverName = server;
            this.username = user;
            this.database = db;
        }

        public void Connect(string password)
        {
            var cond = String.Format("Host={0};Username={1};Password={2};Database={3}", serverName, username, password, database);
            this.dbconnection = new NpgsqlConnection(cond);
        }

        public void OpenConnectoin()
        {
            Console.WriteLine("Connection Open to {0}@{1}", database, serverName);
            dbconnection.Open();
        }

        public void CloseConnection()
        {
            Console.WriteLine("Connection Closed to {0}@{1}", database, serverName);
            dbconnection.Close();
        }

        public string[] Query(string query)
        {
            Console.WriteLine("Exectuing the following query on {0}@{1}:", serverName, database);
            Console.WriteLine(query);
            List<string> data = new List<string>();
            var cmd = new NpgsqlCommand(query, dbconnection);
            var reader = cmd.ExecuteReader();
            using (cmd)
            using (reader)
                while (reader.Read())
                    data.Add(reader.GetString(0));
            return data.ToArray();
        }
    }
}
