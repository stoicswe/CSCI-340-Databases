import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Properties;

public class DBConnect {

    private Connection conn;

    public DBConnect(){
    }

    public void setConnection(String database, String user, String password){
        //‘postgresql://cslab:TacoSh%40ck@localhost:5432/cooking'
        //String format = String.format("jdbc:postgresql://localhost/%s", database);
        String format = String.format("postgresql://cslab:TacoSh%40ck@localhost:5432/cooking", database);
        Properties props = new Properties();
        props.setProperty("user", user);
        props.setProperty("password", password);
        props.setProperty("ssl","true");
        try {
            this.conn = DriverManager.getConnection(format, props);
            this.conn.setAutoCommit(true);
        } catch (SQLException e){
            System.out.println(e.getMessage());
        }
    }

    public Connection getConnection(){
        return this.conn;
    }

    public void closeConnection() {
        try {
            this.conn.close();
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
    }
}
