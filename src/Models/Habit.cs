﻿// -------------------------------------------------------------------------------------------------
// HabitLogger.Models.Habit
// -------------------------------------------------------------------------------------------------
// Data model for habits with properties for its id, name, and unit of measurement.
// -------------------------------------------------------------------------------------------------

namespace HabitLogger.Models;
internal class Habit
{
    #region Properties
    public int Id { get; set; }
    public string HabitName { get; set; } = string.Empty;
    public string UnitOfMeasurement { get; set; } = string.Empty;

    #endregion
}
