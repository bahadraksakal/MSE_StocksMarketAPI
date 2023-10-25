using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStocksService
{
    public class ApiResponseStock
    {
        public int id { get; set; }
        public string kod { get; set; }
        public string ad { get; set; }
        public string tip { get; set; }
    }
}
