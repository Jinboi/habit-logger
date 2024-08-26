// -------------------------------------------------------------------------------------------------
// HabitLogger.Models.HabitLogShowData
// -------------------------------------------------------------------------------------------------
// Data model to show data of habig logs with properties for its id, habit id, habit name, date, 
// quantity and unit of measurement.
// -------------------------------------------------------------------------------------------------

namespace HabitLogger.Models;
internal class HabitLogShowData
{
    #region Properties
    public int Id { get; set; }
    public string HabitName { get; set; } = string.Empty;
    public int HabitId { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string UnitOfMeasurement { get; set; } = string.Empty;

    #endregion
}
