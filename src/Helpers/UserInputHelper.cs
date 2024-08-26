// -------------------------------------------------------------------------------------------------
// HabitLogger.Helpers.UserInputHelper
// -------------------------------------------------------------------------------------------------
// Helper class to get user inputs throw exceptions if they're invalid.
// -------------------------------------------------------------------------------------------------

using HabitLogger.Interfaces;
using System.Globalization;

namespace HabitLogger.Helpers;
internal class UserInputHelper
{
    #region Methods: Internal Static
    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format:yyyy-MM-dd). Type 0 to return to main menu");

        string dateInput = Console.ReadLine();

        if (dateInput == "0") UserInterface.ViewMenu();

        while (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: yyyy-MM-dd). Type 0 to return to main menu or try again.\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }
    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") UserInterface.ViewMenu();

        while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

    #endregion
}
