using HabitLogger.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger;
internal class AppEngine
{
    private static readonly DbContext DbContext = new();

    internal static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();
            var selectAllCmd = connection.CreateCommand();
            // Join logs with habits to get habit names
            selectAllCmd.CommandText = @"
            SELECT l.Id, l.Date, l.Quantity, h.Name, h.UnitOfMeasurement
            FROM logs l
            JOIN habits h ON l.HabitId = h.Id";

            List<LogRecord> logRecords = new();

            SqliteDataReader reader = selectAllCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    logRecords.Add(
                    new LogRecord
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2),
                        HabitName = reader.GetString(3),
                        UnitOfMeasurement = reader.GetString(4)
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();

            Console.WriteLine("----------------------------------------------------\n");
            foreach (var log in logRecords)
            {
                Console.WriteLine($"{log.Id} - {log.Date.ToString("dd-MMM-yyyy")} - {log.Quantity} {log.UnitOfMeasurement} ({log.HabitName})");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }
    }

    internal static void Insert()
    {
        // Fetch all habits and allow the user to select one
        List<Habit> habits = UserInputHelper.GetAllHabits();
        Console.WriteLine("\nSelect a habit to track:");
        for (int i = 0; i < habits.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {habits[i].Name} ({habits[i].UnitOfMeasurement})");
        }

        int habitIndex = UserInputHelper.GetNumberInput("\nEnter the number of the habit:") - 1;
        if (habitIndex < 0 || habitIndex >= habits.Count)
        {
            Console.WriteLine("\nInvalid selection.");
            return;
        }

        var selectedHabit = habits[habitIndex];

        string date = UserInputHelper.GetDateInput();
        int quantity = UserInputHelper.GetNumberInput($"\n\nPlease insert number of {selectedHabit.UnitOfMeasurement} (no decimals allowed)\n\n");

        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
                $"INSERT INTO logs (HabitId, Date, Quantity) VALUES(@habitId, @date, @quantity)";

            insertCmd.Parameters.AddWithValue("@habitId", selectedHabit.Id);
            insertCmd.Parameters.AddWithValue("@date", date);
            insertCmd.Parameters.AddWithValue("@quantity", quantity);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    internal static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = UserInputHelper.GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();
            var deleteCmd = connection.CreateCommand();
            // Correct the table name to 'logs'
            deleteCmd.CommandText = $"DELETE from logs WHERE Id = @id";

            // Added Parameterized Queries
            deleteCmd.Parameters.AddWithValue("@id", recordId);

            int rowCount = deleteCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                Delete();
            }
            else
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted.\n\n");
            }

            connection.Close();
        }

        UserInterface.ViewMenu();
    }
    internal static void Update()
    {
        GetAllRecords();

        var recordId = UserInputHelper.GetNumberInput("\n\nPlease type the Id of the record you would like to update. Type 0 to go back to Main Menu.\n\n");

        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM logs WHERE Id = @id)"; // Correct the table name to 'logs'

            // Added Parameterized Queries
            checkCmd.Parameters.AddWithValue("@id", recordId);

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                connection.Close();
                Update();
            }
            else
            {
                string date = UserInputHelper.GetDateInput();
                int quantity = UserInputHelper.GetNumberInput("\n\nPlease insert number of units for this habit (no decimals allowed)\n\n");

                var updateCmd = connection.CreateCommand();
                updateCmd.CommandText = $"UPDATE logs SET Date = @date, Quantity = @quantity WHERE Id = @id"; // Correct the table name to 'logs'

                // Added Parameterized Queries
                updateCmd.Parameters.AddWithValue("@date", date);
                updateCmd.Parameters.AddWithValue("@quantity", quantity);
                updateCmd.Parameters.AddWithValue("@id", recordId);

                updateCmd.ExecuteNonQuery();

                Console.WriteLine($"\n\nRecord with Id {recordId} was updated successfully.\n\n");
            }

            connection.Close();
        }
    }

    internal static void AddNewHabit()
    {
        Console.WriteLine("\nEnter the name of the new habit:");
        string habitName = Console.ReadLine();

        Console.WriteLine("\nEnter the unit of measurement for this habit (e.g., glasses, minutes):");
        string unitOfMeasurement = Console.ReadLine();

        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
                $"INSERT INTO habits (Name, UnitOfMeasurement) VALUES(@name, @unit)";

            insertCmd.Parameters.AddWithValue("@name", habitName);
            insertCmd.Parameters.AddWithValue("@unit", unitOfMeasurement);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }

        Console.WriteLine($"\nHabit '{habitName}' added successfully.");
    }

}
