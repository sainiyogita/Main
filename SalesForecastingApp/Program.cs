using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForecastingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            Console.WriteLine("Enter the year for sales data:");
            int year = int.Parse(Console.ReadLine());

            SalesData salesData = dbHelper.GetSalesByYear(year);
            Console.WriteLine($"Total Sales for {year}: {salesData.TotalSales}");
            foreach (var stateSales in salesData.SalesByState)
            {
                Console.WriteLine($"{stateSales.Key}: {stateSales.Value}");
            }

            Console.WriteLine("Enter the percentage increase for forecast:");
            decimal percentage = decimal.Parse(Console.ReadLine());

            SalesData forecastedSales = dbHelper.ApplyPercentageIncrease(salesData, percentage);
            Console.WriteLine($"Total Forecasted Sales: {forecastedSales.TotalSales}");
            foreach (var stateSales in forecastedSales.SalesByState)
            {
                Console.WriteLine($"{stateSales.Key}: {stateSales.Value}");
            }

            Console.WriteLine("Enter the file path to export the CSV:");
            string filePath = Console.ReadLine();
            dbHelper.ExportToCsv(forecastedSales, filePath, percentage);
            Console.WriteLine("Exported to CSV file successfully.");
        }
    }
}