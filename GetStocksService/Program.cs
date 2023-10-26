using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace GetStocksService
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string BaseUrl = "https://localhost:7261/api/Stock/SetStocks";

            string apiUrl = "http://bigpara.hurriyet.com.tr/api/v1/hisse/list";
            string apiUrlDetail = "https://bigpara.hurriyet.com.tr/api/v1/borsa/hisseyuzeysel/";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    Console.WriteLine("Hisse senetleri alınıyor bu işlem 1dk-3dk arası sürebilmektedir...");
                    // Web API'den verileri çek
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    // JSON verilerini çek ve deserialize et
                    string json = await response.Content.ReadAsStringAsync();
                    ApiResponse result = JsonSerializer.Deserialize<ApiResponse>(json);

                    if (result != null && result.code == "0")
                    {
                        Methods methods = new Methods();
                        List<StockDTO> stockDTOs = await methods.GetStockDetail(httpClient,result,apiUrlDetail);
                        Console.WriteLine("Tüm veriler çekildi. İşlem Başarılı.");

                        Console.WriteLine("Alınan Veriler:");
                        foreach (var stockDTO in stockDTOs)
                        {
                            Debug.WriteLine($"Name: {stockDTO.StockName} - Price: {stockDTO.Price} - Date: {stockDTO.Date}");
                        }
                        
                        //JSON verisini hazırla
                        string jsonstockDTOs = JsonSerializer.Serialize(stockDTOs);
                        var content = new StringContent(jsonstockDTOs, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseBase = await httpClient.PostAsync(BaseUrl, content);

                        if (responseBase.IsSuccessStatusCode)
                        {
                            Console.WriteLine("POST isteği başarılı.");
                        }
                        else
                        {
                            Console.WriteLine("POST isteği başarısız. Hata kodu: " + responseBase.StatusCode);
                            string errorResponse = await responseBase.Content.ReadAsStringAsync();
                            Console.WriteLine("Hata Mesajı: " + errorResponse);
                        }

                    }
                    else
                    {
                        Console.WriteLine("API'den geçersiz bir yanıt alındı.");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HTTP isteği sırasında hata oluştu: {e.Message}");
                }
                finally
                {
                    Environment.Exit(0);
                }
            }
        }
        
    }
    
    class Methods
    {
        public async Task<List<StockDTO>> GetStockDetail(HttpClient httpClient, ApiResponse result, string apiUrlDetail)
        {
            HttpResponseMessage responseDetail;
            string jsonStockDetail;
            ApiResponseDetail apiResponseDetail;
            //Paralel kullanınca bazı senetleri 2 kere veya 3 kere yazıyordu, durumu çözmek için uğraştım fakat tam anlamıyla çözemedim.
            //Dictionary ile aynı işlem 2-3 kere olsa bile bunu engelleyebiliyorum.
            //paralel kullanmaz isem işlem 2-3 dk sürüyor paralel ile 20 saniye.
            Dictionary<string, StockDTO> stockDTOsKeyValuePairs = new Dictionary<string, StockDTO>();

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 20
            };

            await Parallel.ForEachAsync(result.data, options, async (dataItem, ct) =>
            {
                if (dataItem.tip == "Hisse")
                {
                    try
                    {

                        responseDetail = await httpClient.GetAsync(apiUrlDetail + dataItem.kod);
                        if (responseDetail.IsSuccessStatusCode)
                        {
                            jsonStockDetail = await responseDetail.Content.ReadAsStringAsync();
                            //Console.WriteLine(jsonStockDetail);

                            apiResponseDetail = JsonSerializer.Deserialize<ApiResponseDetail>(jsonStockDetail);
                            if (apiResponseDetail.data.hisseYuzeysel != null && apiResponseDetail.data.hisseYuzeysel.satis != 0)
                            {
                                lock (stockDTOsKeyValuePairs)
                                {
                                    stockDTOsKeyValuePairs.TryAdd(apiResponseDetail.data.hisseYuzeysel.sembol, new StockDTO
                                    {
                                        StockName = apiResponseDetail.data.hisseYuzeysel.sembol,
                                        Price = apiResponseDetail.data.hisseYuzeysel.satis,
                                        Date = DateTime.Now,
                                        StockStatus = false
                                    });
                                }
                            }
                        }                       
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Hisse Detay Çekerken Hata: " + ex.Message);
                    }
                }
            });

            return stockDTOsKeyValuePairs.Values.ToList();
        }
    }
}
