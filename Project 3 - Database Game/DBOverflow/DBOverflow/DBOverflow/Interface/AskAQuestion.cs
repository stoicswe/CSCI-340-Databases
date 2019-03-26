using System;
using DBOverflow.DBConnection;
namespace DBOverflow.DBOverflow.Interface
{
    class AskAQuestionUI : UIObject
    {
        string username;
        int userID;
        DBConnectionHandler dBConnectionHandler;

        public AskAQuestionUI(string username, int userID, DBConnectionHandler dBConnectionHandler)
        {
            this.username = username;
            this.userID = userID;
            this.dBConnectionHandler = dBConnectionHandler;
        }

        public override string[] Show()
        {
            Console.Clear();
            Console.WriteLine("Signed in as: {0}", this.username);
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Please enter your question (blank for exit):");
            Console.Write(">");
            var qu = Console.ReadLine();
            // use this implementation or do all the sql stuff in this class
            //INSERT INTO questions(question, ownerid) VALUES('', 12);
            dBConnectionHandler.Query(String.Format("INSERT INTO questions(question,ownerid) VALUES('{0}', {1});", qu, this.userID.ToString()), 0);
            return new string[] { qu };
        }
    }
}
