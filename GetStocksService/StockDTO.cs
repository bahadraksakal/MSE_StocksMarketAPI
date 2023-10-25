using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStocksService
{
    public class StockDTO
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }
    }
}
