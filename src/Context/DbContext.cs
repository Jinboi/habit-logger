// -------------------------------------------------------------------------------------------------
// HabitLogger.Context.DbContext
// -------------------------------------------------------------------------------------------------
// Sets up database source and creates database and seed data if there's no database/seed data.
// -------------------------------------------------------------------------------------------------

using Microsoft.Data.Sqlite;

namespace HabitLogger.Context;
public class DbContext
{
    #region Fields

    private const string connectionString = @"Data Source=HabitLogger.db";

    #endregion
    #region Methods: public
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

    #endregion
    #region Methods: private
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
            new { Name = "Drink Water", UnitOfMeasurement = "Glasses" },
            new { Name = "Exercise", UnitOfMeasurement = "Minutes" },
            new { Name = "Read", UnitOfMeasurement = "Pages" }
        };

            var habitInsertCmd = connection.CreateCommand();
            foreach (var habit in habitNames)
            {
                habitInsertCmd.CommandText = "INSERT INTO habits (Name, UnitOfMeasurement) VALUES (@name, @unit)";
                habitInsertCmd.Parameters.AddWithValue("@name", habit.Name);
                habitInsertCmd.Parameters.AddWithValue("@unit", habit.UnitOfMeasurement);
                habitInsertCmd.ExecuteNonQuery();
                habitInsertCmd.Parameters.Clear();
            }

            // Retrieve habit data for use in log entries
            var habitDataCmd = connection.CreateCommand();
            habitDataCmd.CommandText = "SELECT Id, Name, UnitOfMeasurement FROM habits";
            SqliteDataReader habitReader = habitDataCmd.ExecuteReader();

            var habits = new List<(int Id, string Name, string Unit)>();
            while (habitReader.Read())
            {
                habits.Add((
                    Id: habitReader.GetInt32(0),
                    Name: habitReader.GetString(1),
                    Unit: habitReader.GetString(2)
                ));
            }

            // Insert random log entries for the habits
            var random = new Random();
            var logInsertCmd = connection.CreateCommand();

            for (int i = 0; i < 100; i++)
            {
                var habit = habits[random.Next(habits.Count)];
                string date = DateTime.Today.AddDays(-random.Next(30)).ToString("yyyy-MM-dd");
                int quantity = random.Next(1, 11);

                logInsertCmd.CommandText = "INSERT INTO logs (HabitId, Date, Quantity) VALUES (@habitId, @date, @quantity)";
                logInsertCmd.Parameters.AddWithValue("@habitId", habit.Id);
                logInsertCmd.Parameters.AddWithValue("@date", date);
                logInsertCmd.Parameters.AddWithValue("@quantity", quantity);
                logInsertCmd.ExecuteNonQuery();
                logInsertCmd.Parameters.Clear();
            }
        }
    }

    #endregion
}
