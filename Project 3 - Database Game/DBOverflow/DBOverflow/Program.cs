using System;
using DBOverflow.DBConnection;

namespace DBOverflow
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Testing DB Connection...");
            var dbh = new DBConnectionHandler("csgpu1:5432", "cslab", "dboverflow");
            Console.Write("Please enter password for connecting: ");
            string p = Console.ReadLine();
            dbh.Connect(p);
            p = null;
            dbh.OpenConnectoin();
            dbh.CloseConnection();
        }
    }
}
