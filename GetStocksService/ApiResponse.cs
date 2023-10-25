using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStocksService
{
    public class ApiResponse
    {
        public string code { get; set; }
        public List<ApiResponseStock> data { get; set; }
    }
}
