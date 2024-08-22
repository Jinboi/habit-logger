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
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        DATE TEXT,
                        Quantity INTEGER                      
                        )";
            tableCmd.ExecuteNonQuery();
        }
    }
}
