using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStocksService
{
    public class StockDTO
    {
        public string StockName { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

        public bool StockStatus { get; set; }
    }
}
