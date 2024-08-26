// -------------------------------------------------------------------------------------------------
// HabitLogger.Interfaces.UserInterfaces
// -------------------------------------------------------------------------------------------------
// UserInterface for the application. Calls HabitLoggerServce when user input is entered.
// -------------------------------------------------------------------------------------------------

using HabitLogger.Services;

namespace HabitLogger.Interfaces;
internal interface UserInterface
{
    static void ViewMenu()
    {
        Console.Clear();

        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("Type 5 to Add New Habit.");
            Console.WriteLine("Type 6 to View Performance Report.");
            Console.WriteLine("Type 7 to View Yearly Report.");

            Console.WriteLine("------------------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    HabitLoggerService.ShowData();
                    break;
                case "2":
                    HabitLoggerService.Insert();
                    break;
                case "3":
                    HabitLoggerService.Delete();
                    break;
                case "4":
                    HabitLoggerService.Update();
                    break;
                case "5":
                    HabitLoggerService.AddNewHabit();
                    break;
                case "6":
                    HabitLoggerService.GenerateHabitPerformanceReport();
                    break;
                case "7":
                    HabitLoggerService.GenerateYearlyHabitSummary();
                    break;

                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}
