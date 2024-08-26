// -------------------------------------------------------------------------------------------------
// HabitLogger.Program
// -------------------------------------------------------------------------------------------------
// Insert point of the HabitLogger application. Sets up DbContext and create seed data if it has not 
// been created already before ViewMenu.
// -------------------------------------------------------------------------------------------------

using HabitLogger.Interfaces;
using HabitLogger.Context;

namespace HabitLogger;
class Program
{
    static void Main(string[] args)
    {
        var dbContext = new DbContext();
        dbContext.EnsureDatabaseCreated();

        UserInterface.ViewMenu();
    }
}
