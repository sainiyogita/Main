using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForecastingApp
{
    public class SalesData
    {
        public decimal TotalSales { get; set; }
        public Dictionary<string, decimal> SalesByState { get; set; }

        public SalesData()
        {
            SalesByState = new Dictionary<string, decimal>();
        }
    }
}
