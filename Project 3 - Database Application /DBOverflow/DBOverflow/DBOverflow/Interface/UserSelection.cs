using System;
using System.Collections.Generic;
using DBOverflow.DBConnection;
using System.Threading;

namespace DBOverflow.DBOverflow.Interface
{
    class UserSelectionUI : UIObject
    {
        private DBConnectionHandler dbhandler;

        public UserSelectionUI(DBConnectionHandler handle)
        {
            this.dbhandler = handle;
        }

        public override string[] Show()
        {
            var uids = dbhandler.Query("SELECT id FROM users;", 0);
            HashSet<int> uidset = new HashSet<int>();
            Dictionary<int, string> unames = new Dictionary<int, string>();
            Dictionary<string, int> uidd = new Dictionary<string, int>();
            foreach (string id in uids)
            {
                var nid = Convert.ToInt32(id);
                uidset.Add(nid);
                var uname = dbhandler.Query(String.Format("SELECT id,name FROM users WHERE id={0}", id), 1)[0];
                unames.Add(nid, uname);
                try
                {
                    uidd.Add(uname, nid);
                }
                catch { }
            }
            //have a spot here for someone to type in their username
            while (true)
            {
                Console.Clear();
                Console.WriteLine(new string('-', Console.WindowWidth));
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Please enter your username or ID:");
                Console.Write(">");
                var un = Console.ReadLine();
                int isid = -1;
                string sid = "";
                try
                {
                    isid = Convert.ToInt32(un);
                }
                catch
                {
                    sid = un;
                }
                if (isid != -1)
                {
                    if (uidset.Contains(isid))
                    {
                        return new string[] { isid.ToString(), unames[isid] };
                    }
                }
                if (sid != "")
                {
                    if (uidd.ContainsKey(sid))
                    {
                        return new string[] { uidd[sid].ToString(), sid };
                    }
                }
                Console.WriteLine("INVALID USERID OR USERNAME!");
                Thread.Sleep(1000);
            }
        }
    }
}
