// -------------------------------------------------------------------------------------------------
// HabitLogger.Models.HabitLog
// -------------------------------------------------------------------------------------------------
// Data model for logs of habits with properties for its id, habit id, date and quantity.
// -------------------------------------------------------------------------------------------------

namespace HabitLogger.Models;
public class HabitLog
{
    #region Properties
    public int Id { get; set; }
    public int HabitId { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    #endregion
}

