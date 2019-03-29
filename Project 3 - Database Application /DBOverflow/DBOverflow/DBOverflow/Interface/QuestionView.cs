using System;
using System.Collections;
using System.Collections.Generic;
using DBOverflow.DBConnection;
namespace DBOverflow.DBOverflow.Interface
{
    class QuestionViewUI : UIObject
    {
        string userName;
        int userID;
        DBConnectionHandler dBConnectionHandler;

        public QuestionViewUI(string username, int userid, DBConnectionHandler dBConnectionHandler)
        {
            this.userName = username;
            this.userID = userid;
            this.dBConnectionHandler = dBConnectionHandler;
        }

        public override string[] Show()
        {
            throw new NotImplementedException();
        }

        public string[] Show(string questionID)
        {
            var questionText = dBConnectionHandler.Query(String.Format("SELECT id,question FROM questions WHERE id={0};", questionID), 0)[0];
            return Show(questionID, questionText);
        }

        public string[] Show(string questionID, string questionText)
        {
            var questionOwner = dBConnectionHandler.Query(String.Format("SELECT * FROM questions WHERE id={0};", questionID), 2)[0];
            var questionOwnerName = dBConnectionHandler.Query(String.Format("SELECT * FROM users WHERE id={0};", questionOwner), 1)[0];
            questionText = dBConnectionHandler.Query(String.Format("SELECT id,question FROM questions WHERE id={0};", questionID), 1)[0];

            while (true)
            {
                Dictionary<string, string[]> answers = new Dictionary<string, string[]>();
                var aids = dBConnectionHandler.Query(String.Format("SELECT * FROM answers WHERE questionid={0};", questionID), 0);
                string questionUpvote = "0";
                string questionDownvote = "0";
                try {
                    questionUpvote = dBConnectionHandler.Query(String.Format("SELECT * FROM votecounts WHERE id={0};", questionID), 1)[0];
                    questionDownvote = dBConnectionHandler.Query(String.Format("SELECT * FROM votecounts WHERE id={0};", questionID), 2)[0];
                }catch{}
                //answer
                //ownerid
                if (aids.Length != 0)
                {
                    foreach (string aid in aids)
                    {
                        var atext = dBConnectionHandler.Query(String.Format("SELECT * FROM answers WHERE id={0};", aid), 1);
                        var owna = dBConnectionHandler.Query(String.Format("SELECT * FROM answers WHERE id={0};", aid), 2);
                        var uname = dBConnectionHandler.Query(String.Format("SELECT * FROM users WHERE id={0};", owna[0]), 1);
                        var ansd = new string[] { atext[0], uname[0] };
                        answers.Add(aid, ansd);
                    }
                }
                Console.Clear();
                Console.WriteLine("Currently signed in as: {0}", this.userName);
                Console.WriteLine(new String('-', Console.WindowWidth));
                Console.WriteLine("{0}:", questionOwnerName);
                Console.WriteLine(questionText);
                Console.WriteLine();
                Console.WriteLine("Upvotes: {0} Downvotes: {1}", questionUpvote, questionDownvote);
                Console.WriteLine(new String('-', Console.WindowWidth));
                if (aids.Length != 0)
                {
                    foreach (string aid in aids)
                    {
                        Console.WriteLine("{1} : {0}", answers[aid][0], answers[aid][1]);
                    }
                }
                else
                {
                    Console.WriteLine("No answers to this question, you can be the first to answer it!");
                }
                Console.WriteLine(new String('-', Console.WindowWidth));
                Console.WriteLine("'c' to comment, 'v' to vote, or 'q' to exit.");
                var k = Console.ReadKey().Key;
                if(k == ConsoleKey.C)
                {
                    Console.Write(">");
                    var ans = Console.ReadLine();
                    dBConnectionHandler.Query(String.Format("INSERT INTO answers (answer, questionid, ownerid) VALUES ('{0}',{1},{2});", ans, questionID, this.userID.ToString()), 0);
                }
                if(k == ConsoleKey.V)
                {
                    Console.WriteLine("Use the U or D keys on your keyboard to vote (any other key will cancel the vote).");
                    var v = Console.ReadKey().Key;
                    if(v == ConsoleKey.U)
                    {
                        dBConnectionHandler.Query(String.Format("INSERT INTO votes (userid, questionid, vote) VALUES ({0},{1},'{2}');", this.userID.ToString(), questionID, "True"), 0);
                    }
                    if(v == ConsoleKey.D)
                    {
                        dBConnectionHandler.Query(String.Format("INSERT INTO votes (userid, questionid, vote) VALUES ({0},{1},'{2}');", this.userID.ToString(), questionID, "False"), 0);
                    }
                }
                if(k == ConsoleKey.Q)
                {
                    break;
                }
            }
            return new string[] { "back" };
        }
    }
}
