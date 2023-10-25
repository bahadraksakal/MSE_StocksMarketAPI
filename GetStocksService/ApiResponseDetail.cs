using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStocksService
{
    public class ApiResponseDetail
    {
        public string code { get; set; }
        public ApiResponseDetailInfo data { get; set; }
    }
}
