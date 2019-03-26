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
        private int port;
        private NpgsqlConnection dbconnection;
        private bool verbose;

        public DBConnectionHandler(string server, int port, string user, string db, bool verbose = false)
        {
            this.serverName = server;
            this.port = port;
            this.username = user;
            this.database = db;
            this.verbose = verbose;
        }

        public void Connect()
        {
            var cond = String.Format("Host={0};Port={1};Username={2};Password={3};Database={4}", serverName, port, username, getPassword(), database);
            this.dbconnection = new NpgsqlConnection(cond);
        }

        public void OpenConnectoin()
        {
            if (verbose) { Console.WriteLine("Connection Open to {0}@{1}:{2}", database, serverName, port); }
            dbconnection.Open();
        }

        public void CloseConnection()
        {
            if (verbose) { Console.WriteLine("Connection Closed to {0}@{1}:{2}", database, serverName, port); }
            dbconnection.Close();
        }

        public string[] Query(string query, int column)
        {
            if (verbose)
            {
                Console.WriteLine("Exectuing the following query on {0}@{1}:{2}", database, serverName, port);
                Console.WriteLine(query);
            }
            List<string> data = new List<string>();
            try
            {
                using (var cmd = new NpgsqlCommand(query, dbconnection))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        data.Add(reader.GetValue(column).ToString());
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return data.ToArray();
        }

        private string sanitizeQuery(string query)
        {
            //make sure nothing stupid happens with the queries.
            return query;
        }

        private string getPassword()
        {
            Console.Write("Please enter password for connecting: ");
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            Console.WriteLine();
            return pass;
        }
    }
}
