using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStocksService
{
    public class ApiResponseDetailStock
    {
        public string sembol {  get; set; }
        public DateTime tarih { get; set; }
        public float satis {  get; set; }
    }
}
