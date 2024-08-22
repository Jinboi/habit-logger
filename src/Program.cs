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
