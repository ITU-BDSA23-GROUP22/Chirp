using Microsoft.Data.Sqlite;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;

//Code in createDB is partially inspired by 
// https://stackoverflow.com/questions/15292880/create-sqlite-database-and-table

//Code in readDB is partially inspired by
// https://stackoverflow.com/questions/38096399/pulling-all-data-from-sqlite-table-column-using-c-sharp-iteration


class DBFacade
{
    public static void createDB()
    {

        SQLiteConnection.CreateFile("cheep.sqlite");

        string connectionString = "Data Source=cheep.sqlite;Version=3";
        using (SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString))
        {
            m_dbConnection.Open();
            string sql = "DROP TABLE IF EXISTS user";
            using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
            {
                command.ExecuteNonQuery();
            }

            sql = "DROP TABLE IF EXISTS user";
            using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
            {
                command.ExecuteNonQuery();
            }

            sql = "DROP TABLE IF EXISTS message";
            using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE TABLE IF NOT EXISTS user (user_id integer primary key autoincrement, username string not null, email string not null, pw_hash string not null)";
            using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE TABLE IF NOT EXISTS message (message_id integer primary key autoincrement, author_id integer not null, text string not null, pub_date integer)";
            using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
            {
                command.ExecuteNonQuery();
            }

            StreamReader sr = new StreamReader("dump.sql");
            var line = sr.ReadLine();
            while (line != null)
            {
                using (SQLiteCommand command = new SQLiteCommand(line.ToString(), m_dbConnection))
                {
                    command.ExecuteNonQuery();
                }
                line = sr.ReadLine();
            }
            m_dbConnection.Close();
        }






        /*
                sql = "INSERT INTO user VALUES(1,'Roger Histand','Roger+Histand@hotmail.com','AZ0serCHTCtJWR+sQCF4MhhfYLyLuK9tU4bWVy0AOBU=')";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                sql = "INSERT INTO message VALUES(0,10,'They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.',1690895677)";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
        */
    }

    public static List<List<string>> readDB(int page, int amount, string user)
    {
        string connectionString = "Data Source=cheep.sqlite;Version=3";
        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
        m_dbConnection.Open();
        var command = m_dbConnection.CreateCommand();
        if (user != null)
        {
            command.CommandText = @$"
            SELECT u.username, m.text, m.pub_date
            FROM user u
            INNER JOIN message m ON u.user_id = m.author_id
            WHERE u.username = '{user}'
            ORDER BY m.pub_date
            LIMIT ({page} - 1)*{amount}, {amount}";
        }
        else
        {
            command.CommandText = @$"
            SELECT u.username, m.text, m.pub_date
            FROM user u
            INNER JOIN message m ON u.user_id = m.author_id
            ORDER BY m.pub_date
            LIMIT ({page} - 1)*{amount}, {amount}";
        }


        using var reader = command.ExecuteReader();
        List<List<string>> result = new List<List<string>>();

        while (reader.Read())
        {
            List<string> innerList = new List<string>
        {
            reader.GetString(0), // username
            reader.GetString(1), // message text
            reader.GetInt32(2).ToString() // pub_date (assuming it's an integer)
        };
            result.Add(innerList);
        }
        m_dbConnection.Close();
        if (result.Count > 0)
        {
            return result;
        }
        else
        {
            return null;
        }
    }
}