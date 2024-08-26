// -------------------------------------------------------------------------------------------------
// HabitLogger.Controller.HabitLoggerController
// -------------------------------------------------------------------------------------------------
// Handles data manipulation (CRUD operations) and calls HabitLoggerDataAccessor for data retrieval.
// -------------------------------------------------------------------------------------------------

using HabitLogger.Context;
using HabitLogger.Models;

namespace HabitLogger.Controller;
internal class HabitLoggerController
{
    #region Fields

    private readonly DbContext _dbContext;
    private readonly HabitLoggerDataAccessor _dataAccessor;

    #endregion
    #region Constructors
    public HabitLoggerController()
    {
        _dbContext = new DbContext();
        _dataAccessor = new HabitLoggerDataAccessor(_dbContext);
    }

    #endregion
    #region Methods: Public
    public void InsertLog(int habitId, string date, int quantity)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = $"INSERT INTO logs (HabitId, Date, Quantity) VALUES(@habitId, @date, @quantity)";

            insertCmd.Parameters.AddWithValue("@habitId", habitId);
            insertCmd.Parameters.AddWithValue("@date", date);
            insertCmd.Parameters.AddWithValue("@quantity", quantity);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void DeleteLog(int logId)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = $"DELETE from logs WHERE Id = @id";
            deleteCmd.Parameters.AddWithValue("@id", logId);

            deleteCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    public bool LogExists(int logId)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM logs WHERE Id = @id)";
            checkCmd.Parameters.AddWithValue("@id", logId);

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            return checkQuery > 0;
        }
    }
    public void UpdateLog(int logId, string date, int quantity)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE logs SET Date = @date, Quantity = @quantity WHERE Id = @id";

            updateCmd.Parameters.AddWithValue("@date", date);
            updateCmd.Parameters.AddWithValue("@quantity", quantity);
            updateCmd.Parameters.AddWithValue("@id", logId);

            updateCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void AddNewHabit(string habitName, string unitOfMeasurement)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = $"INSERT INTO habits (Name, UnitOfMeasurement) VALUES(@name, @unit)";

            insertCmd.Parameters.AddWithValue("@name", habitName);
            insertCmd.Parameters.AddWithValue("@unit", unitOfMeasurement);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    #endregion
    #region Methods: Internal

    internal List<Habit> GetAllHabits() => _dataAccessor.GetAllHabits();
    internal List<HabitLogShowData> GetAllHabitLogs() => _dataAccessor.GetAllHabitLogs();
    internal List<HabitReport> GetHabitPerformanceReport(int habitId, int year) => _dataAccessor.GetHabitPerformanceReport(habitId, year);
    internal List<HabitReport> GetYearlyHabitSummary(int year) => _dataAccessor.GetYearlyHabitSummary(year);

    #endregion
}