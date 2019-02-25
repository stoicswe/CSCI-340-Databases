import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.List;
import java.util.Set;

class Question{
    int id;
    int score;
}

public class MainClass {
    public static void main(String[] args) {
        DBConnect c = new DBConnect();
        c.setConnection("cooking", "cslab", "TacoSh@ck");
        Connection conn = c.getConnection();
        SQLHandle sql = new SQLHandle(conn);
        //===================================
        //Find the highest scoring question with a unique tag.
        HashMap<String, String> tags = new HashMap<>();
        sql.select("SELECT (id,score,tags) FROM posts ORDER BY score DESC;");
        //System.out.println(sql.getResults());
        ResultSet topScores = sql.getResults();
        try {
            while (topScores.next()) {
                String qt = topScores.getString("tags");
                qt = qt.replace(">", "");
                String[] qts = qt.split("<");
                int qID = topScores.getInt("id");
                int scr = topScores.getInt("score");
                //at this point the tags are split
                for(String t : qts){
                    if(tags.containsKey(t)){
                        tags.put(t, tags.get(t) + " " + Integer.toString(qID));
                    } else {
                        tags.put(t, Integer.toString(qID));
                    }
                }
                String topID = "";
                Set<String> ks = tags.keySet();
                for (String k : ks){
                    if(tags.get(k).length() == 1){
                        topID = tags.get(k);
                        break;
                    }
                }
                System.out.println("The Question ID with the top score and unique tag is: " + topID);
            }
        } catch (SQLException e){
            System.out.println("Error trying to access results for query 1.");
            System.out.println(e);
        }
        //===================================
        //Pick a holiday and try to find the highest scoring answers relevant to that holiday.
        sql.select("SELECT (id,creationdate,score) FROM posts WHERE date(creationdate) = date('10/31/2016') ORDER BY score DESC LIMIT 1;");
        ResultSet r1 = sql.getResults();
        //System.out.println(sql.getResults());
        sql.select("SELECT (id,creationdate,score) FROM posts WHERE date(creationdate) = date('10/31/2017') ORDER BY score DESC LIMIT 1;");
        //System.out.println(sql.getResults());
        ResultSet r2 = sql.getResults();
        sql.select("SELECT (id,creationdate,score) FROM posts WHERE date(creationdate) = date('10/31/2018') ORDER BY score DESC LIMIT 1;");
        //System.out.println(sql.getResults());
        ResultSet r3 = sql.getResults();
        //compare the three and choose the top result.
        int sc1 = 0;
        int sc2 = 0;
        int sc3 = 0;
        try {
            sc1 = r1.getInt("score");
            sc2 = r2.getInt("score");
            sc3 = r3.getInt("score");
        } catch (SQLException e){System.out.println(e);}
        System.out.println("===================================");
        System.out.println("Highest Result");
        if (sc1 < sc2){
            if(sc2 < sc3){
                System.out.println(r3);
            }
            System.out.println((r2));
        } else if(sc2 < sc3){
            if(sc3 < sc1){
                System.out.println(r1);
            }
            System.out.println(r1);
        }
        //===================================
        //Build a graphviz dot file with the relationships between the top 200 users where users are connected if they answered a question another user asked.
        sql.select("SELECT (id,displayname,reputation) FROM users ORDER BY reputation DESC LIMIT 200;");
        //System.out.println(sql.getResults());
        ResultSet results = sql.getResults();
        int numberOfRows = 0;
        try{
            numberOfRows = results.getRow();
        } catch (SQLException e){System.out.println(e);}
        for(int i = 0; i < numberOfRows; i++){
            
        }
        //for each user id in results, check if another user shares a question id, if so...then connect for the graph file.
        //===================================
        c.closeConnection();
    }
}
