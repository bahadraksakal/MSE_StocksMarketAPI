using System.Text;
using System.Text.Json;

namespace GetStocksService
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string BaseUrl = "https://localhost:7261/api/Stock/SetStocks";

            string apiUrl = "http://bigpara.hurriyet.com.tr/api/v1/hisse/list";
            string apiUrlDetay = "https://bigpara.hurriyet.com.tr/api/v1/borsa/hisseyuzeysel/";

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
                        HttpResponseMessage responseDetail;
                        string jsonStockDetail;
                        ApiResponseDetail apiResponseDetail;
                        List<StockDTO> stockDTOs = new List<StockDTO>();

                        foreach (var dataItem in result.data)
                        {
                            if(dataItem.tip == "Hisse") 
                            {
                                try
                                {

                                    responseDetail = await httpClient.GetAsync(apiUrlDetay + dataItem.kod);
                                    response.EnsureSuccessStatusCode();

                                    jsonStockDetail = await responseDetail.Content.ReadAsStringAsync();
                                    //Console.WriteLine(jsonStockDetail);

                                    apiResponseDetail = JsonSerializer.Deserialize<ApiResponseDetail>(jsonStockDetail);
                                    if (apiResponseDetail.data.hisseYuzeysel != null)
                                    {
                                        stockDTOs.Add(new StockDTO
                                        {
                                            StockName = apiResponseDetail.data.hisseYuzeysel.sembol,
                                            Price = apiResponseDetail.data.hisseYuzeysel.satis,
                                            Date = DateTime.Now,
                                            StockStatus = false
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    stockDTOs.Add(new StockDTO
                                    {
                                        StockName = dataItem.kod,
                                        Price = 0.00f,
                                        Date = DateTime.Now,
                                        StockStatus = true
                                    });
                                }
                            }
                        }
                        Console.WriteLine("Tüm veriler çekildi. İşlem Başarılı.");
                        //Console.WriteLine("Alınan Veriler:");

                        //foreach (var stockDTO in stockDTOs)
                        //{
                        //    Console.WriteLine($"Name: {stockDTO.StockName} - Price: {stockDTO.Price} - Date: {stockDTO.Date}");
                        //}
                        // JSON verisini hazırla
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
            }
        }
    }
}
