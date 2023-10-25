using System.Text.Json;

namespace GetStocksService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string apiUrl = "http://bigpara.hurriyet.com.tr/api/v1/hisse/list";
            string apiUrlDetay = "https://bigpara.hurriyet.com.tr/api/v1/borsa/hisseyuzeysel/";

            using (var httpClient = new HttpClient())
            {
                try
                {
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
                        var tasks = result.data.Where(dataItem => dataItem.tip == "Hisse").Select(async dataItem =>
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
                                        Name = apiResponseDetail.data.hisseYuzeysel.sembol,
                                        Price = apiResponseDetail.data.hisseYuzeysel.satis,
                                        Date = DateTime.Now,
                                        Status = false
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                stockDTOs.Add(new StockDTO
                                {
                                    Name = dataItem.kod,
                                    Price = 0.00f,
                                    Date = DateTime.Now,
                                    Status = true
                                });
                            }
                        });
                        await Task.WhenAll(tasks);

                        Console.WriteLine("Alınan Veriler:");

                        foreach (var stockDTO in stockDTOs)
                        {
                            Console.WriteLine($"Name: {stockDTO.Name} - Price: {stockDTO.Price} - Date: {stockDTO.Date}");
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
