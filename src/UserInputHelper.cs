using HabitLogger.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger;
internal class UserInputHelper
{
    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu");

        string dateInput = Console.ReadLine();

        if (dateInput == "0") UserInterface.ViewMenu();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again.\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }
    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") UserInterface.ViewMenu();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
    internal static List<Habit> GetAllHabits()
    {
        List<Habit> habits = new();

        // Create an instance of DbContext
        var dbContext = new DbContext();

        using (var connection = dbContext.CreateConnection())
        {
            connection.Open();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM habits";

            SqliteDataReader reader = selectCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        UnitOfMeasurement = reader.GetString(2)
                    });
                }
            }

            connection.Close();
        }

        return habits;
    }

}
