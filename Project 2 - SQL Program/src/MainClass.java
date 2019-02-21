import java.sql.Connection;
import java.sql.ResultSet;

public class MainClass {
    public static void main(String[] args) {
        DBConnect c = new DBConnect();
        c.setConnection("cooking", "cslab", "TacoSh@ck");
        Connection conn = c.getConnection();
        SQLHandle sql = new SQLHandle(conn);
        //String query = "SELECT * FROM questions";
        //sql.update(query);
        //===================================
        //Find the highest scoring question with a unique tag.
        sql.select("SELECT (id,score) FROM posts ORDER BY score DESC;");
        System.out.println(sql.getResults());
        //===================================
        //Pick a holiday and try to find the highest scoring answers relevant to that holiday.
        sql.select("SELECT (id,creationdate,score) FROM posts WHERE date(creationdate) = date('10/31/2016') ORDER BY score DESC LIMIT 1;");
        System.out.println(sql.getResults());
        sql.select("SELECT (id,creationdate,score) FROM posts WHERE date(creationdate) = date('10/31/2017') ORDER BY score DESC LIMIT 1;");
        System.out.println(sql.getResults());
        sql.select("SELECT (id,creationdate,score) FROM posts WHERE date(creationdate) = date('10/31/2018') ORDER BY score DESC LIMIT 1;");
        System.out.println(sql.getResults());
        //===================================
        //Build a graphviz dot file with the relationships between the top 200 users where users are connected if they answered a question another user asked.
        sql.select("SELECT (id,displayname,reputation) FROM users ORDER BY reputation DESC LIMIT 200;");
        System.out.println(sql.getResults());
        ResultSet results = sql.getResults();
        //===================================
        c.closeConnection();
    }
}
