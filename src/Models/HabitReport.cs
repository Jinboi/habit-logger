// -------------------------------------------------------------------------------------------------
// HabitLogger.Models.HabitReport
// -------------------------------------------------------------------------------------------------
// Data model for reports with properties for its id, habit name, number of entries, total quantity
// and unit of measurement.
// -------------------------------------------------------------------------------------------------

namespace HabitLogger.Models;
internal class HabitReport
{
    #region Properties
    public string Year { get; set; } = string.Empty;
    public string HabitName { get; set; } = string.Empty;
    public string UnitOfMeasurement { get; set; } = string.Empty;
    public int NumberOfEntries { get; set; }
    public int TotalQuantity { get; set; }

    #endregion
}
