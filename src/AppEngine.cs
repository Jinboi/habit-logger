using HabitLogger.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger;
internal class AppEngine
{
    private static readonly DbContext DbContext = new();
    internal static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();

            Console.WriteLine("----------------------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - {dw.Quantity} glasses");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }
    }
    internal static void Insert()
    {
        string date = UserInputHelper.GetDateInput();

        int quantity = UserInputHelper.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no deicmals allowed)\n\n");

        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            $"INSERT INTO drinking_water (date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    internal static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = UserInputHelper.GetNumberInput("\n\nPlease type the Id of the record you want to delete ot type 0 to back to Main Menu\n\n");

        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                Delete();
            }
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

        UserInterface.ViewMenu();
    }
    internal static void Update()
    {
        GetAllRecords();

        var recordId = UserInputHelper.GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to go back to Main Menu.\n\n");

        using (var connection = DbContext.CreateConnection())
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                connection.Close();
                Update();
            }

            string date = UserInputHelper.GetDateInput();

            int quantity = UserInputHelper.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
