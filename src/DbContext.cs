using Microsoft.Data.Sqlite;

namespace HabitLogger;
public class DbContext
{
    private const string connectionString = @"Data Source=HabitLogger.db";
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

            // Seed data if tables are empty
            SeedData(connection);
        }
    }

    private void SeedData(SqliteConnection connection)
    {
        // Check if there are any habits in the database
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM habits";
        long habitCount = (long)checkCmd.ExecuteScalar();

        // If no habits exist, seed the database with initial data
        if (habitCount == 0)
        {
            // Insert habits
            var habitNames = new[]
            {
                new { Name = "Drink Water", Unit = "Glasses" },
                new { Name = "Exercise", Unit = "Minutes" },
                new { Name = "Read", Unit = "Pages" }
            };

            var habitInsertCmd = connection.CreateCommand();
            foreach (var habit in habitNames)
            {
                habitInsertCmd.CommandText = "INSERT INTO habits (Name, UnitOfMeasurement) VALUES (@name, @unit)";
                habitInsertCmd.Parameters.AddWithValue("@name", habit.Name);
                habitInsertCmd.Parameters.AddWithValue("@unit", habit.Unit);
                habitInsertCmd.ExecuteNonQuery();
                habitInsertCmd.Parameters.Clear();
            }

            // Retrieve habit ids
            var habitIdCmd = connection.CreateCommand();
            habitIdCmd.CommandText = "SELECT Id FROM habits";
            SqliteDataReader reader = habitIdCmd.ExecuteReader();
            var habitIds = new List<int>();
            while (reader.Read())
            {
                habitIds.Add(reader.GetInt32(0));
            }

            // Insert random log entries for the habits
            var random = new Random();
            var logInsertCmd = connection.CreateCommand();

            for (int i = 0; i < 100; i++)
            {
                int habitId = habitIds[random.Next(habitIds.Count)];
                string date = DateTime.Today.AddDays(-random.Next(30)).ToString("yyyy-MM-dd");  // Change format to yyyy-MM-dd
                int quantity = random.Next(1, 11);

                logInsertCmd.CommandText = "INSERT INTO logs (HabitId, Date, Quantity) VALUES (@habitId, @date, @quantity)";
                logInsertCmd.Parameters.AddWithValue("@habitId", habitId);
                logInsertCmd.Parameters.AddWithValue("@date", date);
                logInsertCmd.Parameters.AddWithValue("@quantity", quantity);
                logInsertCmd.ExecuteNonQuery();
                logInsertCmd.Parameters.Clear();
            }   
        }
    }
}
