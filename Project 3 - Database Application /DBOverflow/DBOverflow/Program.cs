using System;
using DBOverflow.DBOverflow;

namespace DBOverflow
{
    class Program
    {
        //TABLES:
        //USERS: id - int, name - text, about - text
        //QUESTIONS: id - int, question - text, ownerid - int, creationdate - timestamp
        //ANSWERS: id - int, answer - text, ownerid - int, questionid - int, creationdate - timestamp
        //VOTES: userid - int, questionid - int, vote - boolean
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Welcome to the DBOverflow Application.");
            DBOverflowApplication application = new DBOverflowApplication();
            application.Run();
        }
    }
}
