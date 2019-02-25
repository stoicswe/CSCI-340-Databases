import java.io.PrintWriter;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

class User{
    int id;
    int reputation;
    String displayName;

    public User(int id, int reputation, String displayName){
        this.id = id;
        this.reputation = reputation;
        this.displayName = displayName;
    }
}

class Post{
    int id;
    int parent;
    int owner;

    public Post(int id, int parent, int owner){
        this.id = id;
        this.parent = parent;
        this.owner = owner;
    }
}

public class MainClass {
    public static void main(String[] args) {
        DBConnect c = new DBConnect();
        c.setConnection("cooking", "cslab", "TacoSh@ck");
        Connection conn = c.getConnection();
        SQLHandle sql = new SQLHandle(conn);
        System.out.println("Connection established, working with data...");
        //===================================
        //Find the highest scoring question with a unique tag.
        HashMap<String, String> tags = new HashMap<>();
        String query = "SELECT id,score,tags FROM posts ORDER BY score DESC;";
        System.out.println(query);
        sql.select(query);
        System.out.println("Results received, processing...");
        ResultSet topScores = sql.getResults();
        try {
            while (topScores.next()) {
                String qt = topScores.getString("tags");
                if(qt != null) {
                    String qt2 = qt.replace(">", "");
                    String[] qts = qt2.split("<");
                    int qID = topScores.getInt("id");

                    for (String t : qts) {
                        if (tags.containsKey(t)) {
                            tags.put(t, tags.get(t) + " " + qID);
                        } else {
                            tags.put(t, Integer.toString(qID));
                        }
                    }
                }
            }
            System.out.println("===================================");
             String topID = "NONE";
             Set<String> ks = tags.keySet();
             //System.out.println(ks);
             for (String k : ks){
                 if(tags.get(k).length() == 1 && k != ","){
                     topID = tags.get(k);
                 }
             }
             System.out.println("The Question ID with the top score and unique tag is: " + topID);
        } catch (SQLException e){
            System.out.println("Error trying to access results for query 1.");
            System.out.println(e);
        }
        //===================================
        //Pick a holiday and try to find the highest scoring answers relevant to that holiday.
        sql.select("SELECT id,creationdate,score,title FROM posts WHERE date(creationdate) = date('10/31/2016') ORDER BY score DESC LIMIT 1;");
        ResultSet r1 = sql.getResults();
        sql.select("SELECT id,creationdate,score,title FROM posts WHERE date(creationdate) = date('10/31/2017') ORDER BY score DESC LIMIT 1;");
        ResultSet r2 = sql.getResults();
        sql.select("SELECT id,creationdate,score,title FROM posts WHERE date(creationdate) = date('10/31/2018') ORDER BY score DESC LIMIT 1;");
        ResultSet r3 = sql.getResults();
        //compare the three and choose the top result.
        try {
            r1.next();
            r2.next();
            r3.next();
            int sc1 = 0;
            int sc2 = 0;
            int sc3 = 0;
            sc1 = r1.getInt("score");
            sc2 = r2.getInt("score");
            sc3 = r3.getInt("score");
            System.out.println("===================================");
            System.out.println("Highest Result");
            if (sc1 < sc2) {
                if (sc2 < sc3) {
                    System.out.printf("%d, %s\n", r3.getInt("id"), r3.getString("title"));
                }
                System.out.printf("%d, %s\n", r2.getInt("id"), r2.getString("title"));
            } else if (sc2 < sc3) {
                if (sc3 < sc1) {
                    System.out.printf("ID: %d, TITLE: %s\n", r1.getInt("id"), r1.getString("title"));
                }
                //System.out.printf("%d, %s\n", r1.getInt("id"), r1.getString("title"));
            }
        } catch (SQLException e){
            System.out.println("Error working with the results!");
            System.out.println(e);
        }
        //===================================
        //Build a graphviz dot file with the relationships between the top 200 users where users are connected if they answered a question another user asked.
        //inner join with the top200 users
        System.out.println("===================================");
        sql.select("SELECT id,displayname,reputation FROM users ORDER BY reputation DESC LIMIT 200;");
        //System.out.println(sql.getResults());
        ResultSet top200 = sql.getResults();
        HashMap<Integer, User> users = new HashMap<>();
        try {
            while (top200.next()) {
                String dn = top200.getString("displayname");
                int uid = top200.getInt("id");
                int rep = top200.getInt("reputation");
                users.put(uid, new User(uid, rep, dn));
            }
        } catch (SQLException e){
            System.out.println("Error getting the top 200 users IDs");
            System.out.println(e);
        }
        HashMap<Integer, Post> userPosts = new HashMap<>();
        //owneruserid, parentid, id, title
        sql.select("SELECT owneruserid, parentid, posts.id FROM posts INNER JOIN (SELECT id, displayname, reputation FROM users ORDER BY reputation DESC LIMIT 200) as userData ON posts.owneruserid = userData.id;");
        //for each user id in results, check if another user shares a question id, if so...then connect for the graph file.
        ResultSet tuq = sql.getResults();
        try {
            while (tuq.next()) {
                int owner = tuq.getInt("owneruserid");
                int parent;
                try {
                    parent = tuq.getInt("parentid");
                } catch (Exception e){
                    parent = -1;
                }
                int id = tuq.getInt("id");
                userPosts.put(id, new Post(id, parent, owner));
            }
        }catch (SQLException e){
            System.out.println("Issue parsing the post data!");
            System.out.println(e);
        }
        Set<Integer> userIds = users.keySet();
        Set<Integer> postIds = userPosts.keySet();
        LinkedList<String> connect = new LinkedList<String>();
        for(int id : postIds){
            Post p = userPosts.get(id);
            if(userIds.contains(p.owner)){
                Post p2 = userPosts.get(p.parent);
                if(p2 != null) {
                    if (userIds.contains(p2.owner)) {
                        User u1 = users.get(p.owner);
                        User u2 = users.get(p2.owner);
                        connect.add(u1.displayName + " -- " + u2.displayName);
                    }
                }
            }
        }
        System.out.println(connect);
        try {
            PrintWriter writer = new PrintWriter("user-relationships.txt", "UTF-8");
            writer.println("digraph G {");
            for(String ln : connect){
                writer.println("    " + ln);
            }
            writer.println("}");
            writer.close();
        }catch (Exception e){
            System.out.println("Error writing the graphiz file!");
            System.out.println(e);
        }
        //===================================
        c.closeConnection();
    }
}
