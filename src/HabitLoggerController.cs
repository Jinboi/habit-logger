using HabitLogger.Models;
using HabitLogger.Services;

namespace HabitLogger;
internal class HabitLoggerController
{
    private static readonly HabitLoggerService _habitService = new();
    internal static void GetAllRecords()
    {
        Console.Clear();
        var logRecords = _habitService.GetAllRecords();

        if (logRecords.Count == 0)
        {
            Console.WriteLine("No rows found");
        }

        Console.WriteLine("----------------------------------------------------\n");
        foreach (var log in logRecords)
        {
            Console.WriteLine($"{log.Id} - {log.Date.ToString("dd-MMM-yyyy")} - {log.Quantity} {log.UnitOfMeasurement} ({log.HabitName})");
        }
        Console.WriteLine("----------------------------------------------------\n");
    }

    internal static void Insert()
    {
        List<Habit> habits = _habitService.GetAllHabits();
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

        _habitService.InsertLog(selectedHabit.Id, date, quantity);
    }

    internal static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = UserInputHelper.GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

        if (recordId == 0) return;

        _habitService.DeleteRecord(recordId);
    }

    internal static void Update()
    {
        GetAllRecords();
        var recordId = UserInputHelper.GetNumberInput("\n\nPlease type the Id of the record you would like to update. Type 0 to go back to Main Menu.\n\n");

        if (!_habitService.RecordExists(recordId))
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
            return;
        }

        string date = UserInputHelper.GetDateInput();
        int quantity = UserInputHelper.GetNumberInput("\n\nPlease insert number of units for this habit (no decimals allowed)\n\n");

        _habitService.UpdateRecord(recordId, date, quantity);
        Console.WriteLine($"\n\nRecord with Id {recordId} was updated successfully.\n\n");
    }

    internal static void AddNewHabit()
    {
        Console.WriteLine("\nEnter the name of the new habit:");
        string habitName = Console.ReadLine();

        Console.WriteLine("\nEnter the unit of measurement for this habit (e.g., glasses, minutes):");
        string unitOfMeasurement = Console.ReadLine();

        _habitService.AddNewHabit(habitName, unitOfMeasurement);
        Console.WriteLine($"\nHabit '{habitName}' added successfully.");
    }
}
