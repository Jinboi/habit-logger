namespace HabitLogger.Models;
public class LogRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string HabitName { get; set; }
    public string UnitOfMeasurement { get; set; }
}
