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
        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
        m_dbConnection.Open();

        string sql = "CREATE TABLE IF NOT EXISTS highscores (name varchar(20), score int)";
        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        command.ExecuteNonQuery();
        sql = "Insert into highscores (name) values ('Me')";
        command = new SQLiteCommand(sql, m_dbConnection);
        command.ExecuteNonQuery();

        m_dbConnection.Close();
    }

    public static string readDB()
    {
        string connectionString = "Data Source=cheep.sqlite;Version=3";
        SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
        m_dbConnection.Open();
        var command = m_dbConnection.CreateCommand();
        command.CommandText = @"SELECT * FROM highscores";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            return reader.GetString(0);
        }
        return "Failure";

    }
}