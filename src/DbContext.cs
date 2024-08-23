using Microsoft.Data.Sqlite;

namespace HabitLogger;
public class DbContext
{
    private const string connectionString = @"Data Source=HabbitLogger.db";
    public SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        return connection;
    }
    public void EnsureDatabaseCreated()
    {
        using (var connection = CreateConnection())
        {
            // Create the Habits table
            var habitsTableCmd = connection.CreateCommand();
            habitsTableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    UnitOfMeasurement TEXT                      
                    )";
            habitsTableCmd.ExecuteNonQuery();

            // Create the Logs table
            var logsTableCmd = connection.CreateCommand();
            logsTableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS logs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER,
                    Date TEXT,
                    Quantity INTEGER,
                    FOREIGN KEY(HabitId) REFERENCES habits(Id)                  
                    )";
            logsTableCmd.ExecuteNonQuery();
        }
    }
}
