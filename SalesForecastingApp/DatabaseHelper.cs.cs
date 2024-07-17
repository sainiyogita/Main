using SalesForecastingApp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DatabaseHelper
{
    private string connectionString;

    public DatabaseHelper()
    {
        connectionString = ConfigurationManager.ConnectionStrings["SalesForecastingDB"].ConnectionString;
    }

    public SalesData GetSalesByYear(int year)
    {
        SalesData salesData = new SalesData();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = @"
                SELECT SUM(p.Sales) AS TotalSales, o.State
                FROM Orders o
                JOIN Products p ON o.ProductID = p.ProductID
                LEFT JOIN Returns r ON o.OrderID = r.OrderID
                WHERE YEAR(o.OrderDate) = @Year
                GROUP BY o.State";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Year", year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal totalSales = reader.GetDecimal(0);
                        string state = reader.GetString(1);

                        salesData.TotalSales += totalSales;
                        salesData.SalesByState[state] = totalSales;
                    }
                }
            }
        }
        return salesData;
    }

    public SalesData ApplyPercentageIncrease(SalesData originalSales, decimal percentage)
    {
        SalesData forecastedSales = new SalesData();
        forecastedSales.TotalSales = originalSales.TotalSales * (1 + percentage / 100);

        foreach (var stateSales in originalSales.SalesByState)
        {
            forecastedSales.SalesByState[stateSales.Key] = stateSales.Value * (1 + percentage / 100);
        }

        return forecastedSales;
    }

    public void ExportToCsv(SalesData salesData, string filePath, decimal percentage)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("State,Percentage Increase,Sales Value");

            foreach (var stateSales in salesData.SalesByState)
            {
                writer.WriteLine($"{stateSales.Key},{percentage},{stateSales.Value}");
            }
        }
    }
}