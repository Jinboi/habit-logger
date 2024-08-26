// -------------------------------------------------------------------------------------------------
// HabitLogger.Controller.HabitLoggerDataAccessor
// -------------------------------------------------------------------------------------------------
// Handles data retrieval (all "get" methods).
// -------------------------------------------------------------------------------------------------

using HabitLogger.Context;
using HabitLogger.Models;
using System.Globalization;

namespace HabitLogger.Controller;
internal class HabitLoggerDataAccessor
{
    #region Fields

    private readonly DbContext _dbContext;

    #endregion
    #region Constructors
    public HabitLoggerDataAccessor(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion
    #region Methods: Internal
    internal List<Habit> GetAllHabits()
    {
        var habits = new List<Habit>();

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM habits";

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        HabitName = reader.GetString(1),
                        UnitOfMeasurement = reader.GetString(2)
                    });
                }
            }
        }

        return habits;
    }
    internal List<HabitLogShowData> GetAllHabitLogs()
    {
        var logRecords = new List<HabitLogShowData>();

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var selectAllCmd = connection.CreateCommand();
            selectAllCmd.CommandText = @"
                SELECT l.Id, h.Name AS HabitName, l.Date, l.Quantity, h.UnitOfMeasurement
                FROM logs l
                JOIN habits h ON l.HabitId = h.Id";

            using (var reader = selectAllCmd.ExecuteReader())
            {
                var dateFormats = new[] { "yyyy-MM-dd", "dd-MM-yy" };

                while (reader.Read())
                {
                    if (DateTime.TryParseExact(reader.GetString(2), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        logRecords.Add(new HabitLogShowData
                        {
                            Id = reader.GetInt32(0),
                            HabitName = reader.GetString(1),
                            Date = parsedDate,
                            Quantity = reader.GetInt32(3),
                            UnitOfMeasurement = reader.GetString(4)
                        });
                    }
                    else
                    {
                       Console.WriteLine("wrong");
                    }
                }
            }
        }

        return logRecords;
    }
    internal List<HabitReport> GetHabitPerformanceReport(int habitId, int year)
    {
        var report = new List<HabitReport>();

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            SELECT strftime('%Y', l.Date) AS Year, 
                   h.UnitOfMeasurement AS UnitOfMeasurement, 
                   COUNT(*) AS NumberOfEntries, 
                   SUM(l.Quantity) AS TotalQuantity
            FROM logs l
            JOIN habits h ON l.HabitId = h.Id
            WHERE l.HabitId = @habitId 
              AND strftime('%Y', l.Date) = @year
            GROUP BY strftime('%Y', l.Date), h.UnitOfMeasurement";

            cmd.Parameters.AddWithValue("@habitId", habitId);
            cmd.Parameters.AddWithValue("@year", year.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    report.Add(new HabitReport
                    {
                        Year = reader.GetString(0),
                        UnitOfMeasurement = reader.GetString(1),
                        NumberOfEntries = reader.GetInt32(2),
                        TotalQuantity = reader.GetInt32(3)
                    });
                }
            }
        }

        return report;
    }
    internal List<HabitReport> GetYearlyHabitSummary(int year)
    {
        var report = new List<HabitReport>();

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            SELECT h.Name AS HabitName, 
                   h.UnitOfMeasurement AS UnitOfMeasurement, 
                   COUNT(*) AS NumberOfEntries, 
                   SUM(l.Quantity) AS TotalQuantity
            FROM logs l
            JOIN habits h ON l.HabitId = h.Id
            WHERE strftime('%Y', l.Date) = @year
            GROUP BY h.Name, h.UnitOfMeasurement";

            cmd.Parameters.AddWithValue("@year", year.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    report.Add(new HabitReport
                    {
                        HabitName = reader.GetString(0),
                        UnitOfMeasurement = reader.GetString(1),
                        NumberOfEntries = reader.GetInt32(2),
                        TotalQuantity = reader.GetInt32(3)
                    });
                }
            }
        }

        return report;
    }

    #endregion
}
