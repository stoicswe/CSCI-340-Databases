using System;
using System.Collections;
using System.Collections.Generic;
using DBOverflow.DBConnection;
namespace DBOverflow.DBOverflow.Interface
{
    class QuestionBoardUI : UIObject
    {
        string userName;
        DBConnectionHandler dBConnectionHandler;

        public QuestionBoardUI(string userName, DBConnectionHandler dBConnectionHandler)
        {
            this.userName = userName;
            this.dBConnectionHandler = dBConnectionHandler;
        }

        public override string[] Show()
        {
            throw new NotImplementedException();
        }
        //select question, upvote, downvote from questions inner join votecounts on votecounts.id = question.id where question.id = {0};

        public string[] Show(string type)
        {
            Dictionary<string, string> questions = new Dictionary<string, string>();
            string[] qids = new string[0];
            //work with obtaining the questions and such for various different types of questions here
            if (type == "unanswered")
            {
                //generate some SQL queries that grab all the unanswered questions and relevent data to that
                qids = this.dBConnectionHandler.Query("SELECT * FROM unanswered;", 0);
                foreach (string qid in qids)
                {
                    var qt = this.dBConnectionHandler.Query(String.Format("SELECT * FROM unanswered WHERE id={0};", qid), 2);
                    if (qt[0].Length > 150)
                    {
                        string smqt = qt[0].Substring(0, 150);
                        smqt = smqt + "...";
                        questions.Add(qid, smqt);
                    }
                    else
                    {
                        questions.Add(qid, qt[0]);
                    }
                }

            }

            if(type == "top")
            {
                //grab only the top 25 questions
                qids = this.dBConnectionHandler.Query("SELECT * FROM votecounts ORDER BY upvote DESC LIMIT 25;", 0);
                foreach (string qid in qids)
                {
                    var qt = this.dBConnectionHandler.Query(String.Format("SELECT * FROM questions WHERE id={0};", qid), 1);
                    if (qt[0].Length > 150)
                    {
                        string smqt = qt[0].Substring(0, 150);
                        smqt = smqt + "...";
                        questions.Add(qid, smqt);
                    }
                    else
                    {
                        questions.Add(qid, qt[0]);
                    }
                }
            }

            if(type == "random")
            {
                //grab a random 25 questions
                qids = this.dBConnectionHandler.Query("SELECT * FROM questions;", 0);
                foreach (string qid in qids)
                {
                    var qt = this.dBConnectionHandler.Query(String.Format("SELECT * FROM questions WHERE id={0};", qid), 1);
                    if (qt[0].Length > 150)
                    {
                        string smqt = qt[0].Substring(0, 150);
                        smqt = smqt + "...";
                        questions.Add(qid, smqt);
                    }
                    else
                    {
                        questions.Add(qid, qt[0]);
                    }
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Currently signed in as: {0}", this.userName);
                Console.WriteLine(new String('-', Console.WindowWidth));
                foreach (string qd in qids)
                {
                    Console.WriteLine("{0} | {1}", qd, questions[qd]);
                }
                Console.WriteLine(new String('-', Console.WindowWidth));
                Console.WriteLine("Enter a question id to view the question or 'quit' to exit.");
                Console.Write(">");
                var qid = Console.ReadLine();
                if (qid == "quit")
                {
                    return new string[] { "exit" };
                } else if(qid == "refresh")
                {

                }
                else
                {
                    if (questions.ContainsKey(qid))
                    {
                        return new string[] { "view", qid, questions[qid] };
                    }
                }
            }
        }
    }
}
