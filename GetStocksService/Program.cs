using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace GetStocksService
{
    class Program
    {

        static async Task Main(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();


            string BaseUrl = configuration["Url:BaseUrl"];
            string apiUrl = configuration["Url:ApiUrl"];
            string apiUrlDetail = configuration["Url:ApiUrlDetail"];

            string token = CreateToken(configuration);

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
                        //Console.WriteLine("Alınan Veriler:");
                        //foreach (var stockDTO in stockDTOs)
                        //{
                        //    Debug.WriteLine($"Name: {stockDTO.StockName} - Price: {stockDTO.Price} - Date: {stockDTO.Date}");
                        //}

                        //JSON verisini hazırla
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string jsonstockDTOs = JsonSerializer.Serialize(stockDTOs);
                        var content = new StringContent(jsonstockDTOs, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseBase = await httpClient.PutAsync(BaseUrl, content);

                        if (responseBase.IsSuccessStatusCode)
                        {
                            Console.WriteLine("PUT isteği başarılı. İşlem başarıyla tamamlandı.");
                        }
                        else
                        {
                            Console.WriteLine("PUT isteği başarısız. İşlem tamamlanamadı. Hata kodu: " + responseBase.StatusCode);
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

        public static string CreateToken(IConfiguration _configuration)
        {
            var Claims = new[]
                    {
                    new Claim("userName", "Server"),
                    new Claim("userId", "-1"),
                    new Claim(ClaimTypes.Role, "Server") // buradaki role göre auth oluyor.
                };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SigningKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _configuration["JwtConfig:Issuer"], //token sağlayıcısı kim
                    audience: _configuration["JwtConfig:Audience"],
                    claims: Claims, // tokeni doğruladığımda erişebileceğim gömülü değerler
                    expires: DateTime.Now.AddMinutes(2), // 1 dk sonra token yok olsun
                    notBefore: DateTime.Now, // token geçerliği hemen başlasın
                    signingCredentials: credentials
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
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
                            if (apiResponseDetail.data.hisseYuzeysel != null)
                            {
                                lock (stockDTOsKeyValuePairs)
                                {                                    
                                    stockDTOsKeyValuePairs.TryAdd(apiResponseDetail.data.hisseYuzeysel.sembol, new StockDTO
                                    {
                                        StockName = apiResponseDetail.data.hisseYuzeysel.sembol,
                                        // hisse tavan ise alış için 0 değeri geliyor.
                                        Price = apiResponseDetail.data.hisseYuzeysel.satis == 0 ? apiResponseDetail.data.hisseYuzeysel.alis : apiResponseDetail.data.hisseYuzeysel.satis,
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
