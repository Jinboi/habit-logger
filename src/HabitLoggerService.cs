﻿using HabitLogger.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger.Services;
internal class HabitLoggerService
{
    private readonly DbContext _dbContext;

    public HabitLoggerService()
    {
        _dbContext = new DbContext();
    }

    public List<LogRecord> GetAllRecords()
    {
        List<LogRecord> logRecords = new();

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var selectAllCmd = connection.CreateCommand();
            selectAllCmd.CommandText = @"
            SELECT l.Id, l.Date, l.Quantity, h.Name, h.UnitOfMeasurement
            FROM logs l
            JOIN habits h ON l.HabitId = h.Id";

            SqliteDataReader reader = selectAllCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    logRecords.Add(new LogRecord
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2),
                        HabitName = reader.GetString(3),
                        UnitOfMeasurement = reader.GetString(4)
                    });
                }
            }

            connection.Close();
        }

        return logRecords;
    }

    public void InsertLog(int habitId, string date, int quantity)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = $"INSERT INTO logs (HabitId, Date, Quantity) VALUES(@habitId, @date, @quantity)";

            // Convert date to yyyy-MM-dd format
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yy", CultureInfo.InvariantCulture);
            string formattedDate = parsedDate.ToString("yyyy-MM-dd");

            insertCmd.Parameters.AddWithValue("@habitId", habitId);
            insertCmd.Parameters.AddWithValue("@date", formattedDate);
            insertCmd.Parameters.AddWithValue("@quantity", quantity);

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }
    }


    public void DeleteRecord(int recordId)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = $"DELETE from logs WHERE Id = @id";
            deleteCmd.Parameters.AddWithValue("@id", recordId);

            deleteCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public bool RecordExists(int recordId)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM logs WHERE Id = @id)";
            checkCmd.Parameters.AddWithValue("@id", recordId);

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            return checkQuery > 0;
        }
    }

    public void UpdateRecord(int recordId, string date, int quantity)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE logs SET Date = @date, Quantity = @quantity WHERE Id = @id";

            updateCmd.Parameters.AddWithValue("@date", date);
            updateCmd.Parameters.AddWithValue("@quantity", quantity);
            updateCmd.Parameters.AddWithValue("@id", recordId);

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

    public List<Habit> GetAllHabits()
    {
        List<Habit> habits = new();

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM habits";

            SqliteDataReader reader = selectCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        UnitOfMeasurement = reader.GetString(2)
                    });
                }
            }

            connection.Close();
        }

        return habits;
    }

    // Method to get total quantity logged for a specific habit in a year
    public int GetTotalQuantityForHabitInYear(int habitId, int year)
    {
        int totalQuantity = 0;

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var reportCmd = connection.CreateCommand();
            reportCmd.CommandText = @"
            SELECT SUM(Quantity) 
            FROM logs 
            WHERE HabitId = @habitId 
            AND strftime('%Y', Date) = @year";

            reportCmd.Parameters.AddWithValue("@habitId", habitId);
            reportCmd.Parameters.AddWithValue("@year", year.ToString());

            var result = reportCmd.ExecuteScalar();

            // Check if the result is DBNull
            if (result != DBNull.Value && result != null)
            {
                totalQuantity = Convert.ToInt32(result);
            }

            connection.Close();
        }

        return totalQuantity;
    }

    // Method to get total number of entries for a specific habit in a year

    public int GetTotalEntriesForHabitInYear(int habitId, int year)
    {
        int totalEntries = 0;

        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            var reportCmd = connection.CreateCommand();
            reportCmd.CommandText = @"
            SELECT COUNT(*) 
            FROM logs 
            WHERE HabitId = @habitId 
            AND strftime('%Y', Date) = @year";

            reportCmd.Parameters.AddWithValue("@habitId", habitId);
            reportCmd.Parameters.AddWithValue("@year", year.ToString());

            var result = reportCmd.ExecuteScalar();

            // Check if the result is DBNull
            if (result != DBNull.Value && result != null)
            {
                totalEntries = Convert.ToInt32(result);
            }

            connection.Close();
        }

        return totalEntries;
    }

}
