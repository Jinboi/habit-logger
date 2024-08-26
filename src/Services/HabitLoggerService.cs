// -------------------------------------------------------------------------------------------------
// HabitLogger.Services.HabitLoggerService
// -------------------------------------------------------------------------------------------------
// Provides methods for managing habit logs and generating reports, including inserting, updating, 
// deleting logs, and generating habit performance and yearly summary reports based on the user
// inputs from userInterface.
// -------------------------------------------------------------------------------------------------

using HabitLogger.Models;
using HabitLogger.Controller;
using HabitLogger.Helpers;

namespace HabitLogger.Services;
internal class HabitLoggerService
{
    private static readonly HabitLoggerController _habitController = new();

    #region Methods Internal
    internal static void ShowData()
    {
        Console.Clear();
        var logRecords = _habitController.GetAllHabitLogs();

        if (logRecords.Count == 0)
        {
            Console.WriteLine("No rows found");
        }
        else
        {
            Console.WriteLine("----------------------------------------------------\n");
            foreach (var log in logRecords)
            {
                Console.WriteLine($"{log.Id} - {log.HabitName} - {log.Date.ToString("yyyy-MM-dd")} " +
                    $"- {log.Quantity} {log.UnitOfMeasurement}");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }
    }
    internal static void Insert()
    {
        List<Habit> habits = _habitController.GetAllHabits();

        Console.WriteLine("\nSelect a habit to track:");

        for (int i = 0; i < habits.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {habits[i].HabitName} ({habits[i].UnitOfMeasurement})");
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

        _habitController.InsertLog(selectedHabit.Id, date, quantity);
    }
    internal static void Delete()
    {
        Console.Clear();
        ShowData();

        var logId = UserInputHelper.GetNumberInput(@"\n\nPlease type the Id of the log you want to 
        delete or type 0 to go back to Main Menu\n\n");

        if (logId == 0) return;

        _habitController.DeleteLog(logId);
    }
    internal static void Update()
    {
        ShowData();
        var logId = UserInputHelper.GetNumberInput("\n\nPlease type the Id of the log you would like to update. " +
            "Type 0 to go back to Main Menu.\n\n");

        if (!_habitController.LogExists(logId))
        {
            Console.WriteLine($"\n\nRecord with Id {logId} doesn't exist. \n\n");
            return;
        }

        string date = UserInputHelper.GetDateInput();
        int quantity = UserInputHelper.GetNumberInput("\n\nPlease insert number of units for this habit " +
            "(no decimals allowed)\n\n");

        _habitController.UpdateLog(logId, date, quantity);
        Console.WriteLine($"\n\nRecord with Id {logId} was updated successfully.\n\n");
    }
    internal static void AddNewHabit()
    {
        Console.WriteLine("\nEnter the name of the new habit:");
        string habitName = Console.ReadLine();

        Console.WriteLine("\nEnter the unit of measurement for this habit (e.g., glasses, minutes):");
        string unitOfMeasurement = Console.ReadLine();

        _habitController.AddNewHabit(habitName, unitOfMeasurement);
        Console.WriteLine($"\nHabit '{habitName}' added successfully.");
    }
    internal static void GenerateHabitPerformanceReport()
    {
        Console.WriteLine("\nEnter Habit ID:");
        int habitId = int.Parse(Console.ReadLine());

        Console.WriteLine("\nEnter Year (e.g., 2023):");
        int year = int.Parse(Console.ReadLine());

        var report = _habitController.GetHabitPerformanceReport(habitId, year);

        if (report.Count == 0)
        {
            Console.WriteLine("No data found for the specified habit and year.");
        }
        else
        {
            Console.WriteLine("----------------------------------------------------\n");
            foreach (var entry in report)
            {
                Console.WriteLine($"Year: {entry.Year} - Number of Entries: {entry.NumberOfEntries} " +
                    $"- Total Quantity: {entry.TotalQuantity}  - Unit: {entry.UnitOfMeasurement} ");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }
    }
    internal static void GenerateYearlyHabitSummary()
    {
        Console.WriteLine("\nEnter Year (e.g., 2023):");
        int year = int.Parse(Console.ReadLine());

        var report = _habitController.GetYearlyHabitSummary(year);

        if (report.Count == 0)
        {
            Console.WriteLine("No data found for the specified year.");
        }
        else
        {
            Console.WriteLine("----------------------------------------------------\n");
            foreach (var entry in report)
            {
                Console.WriteLine($"Habit: {entry.HabitName} - Number of Entries: {entry.NumberOfEntries} " +
                    $"- Total Quantity: {entry.TotalQuantity} - Unit: {entry.UnitOfMeasurement}");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }
    }

    #endregion
}