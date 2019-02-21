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
        sql.select("SELECT * FROM unanswered");
        //Find the highest scoring question with a unique tag.
        //Pick a holiday and try to find the highest scoring answers relevant to that holiday.
        //Build a graphviz dot file with the relationships between the top 200 users where users are connected if they answered a question another user asked.
        System.out.println(sql.getResults());
        c.closeConnection();
    }
}
