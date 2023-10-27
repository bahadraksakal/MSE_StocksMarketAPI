# MSE_StocksMarketAPI
*asp.net core projesi 

**
- Hisse senetlerini çekmek için: Önce ana projeyi çalıştırın. Daha sonra GetStockService projesini derleyin ve .exe dosyası üzerinden çalıştırın.
GetStockService tüm hisseleri çekip otomatik olarak veritabanına kayıt edicektir. (Hisse senedi yoksa yeni kayıt ekler,, var ise son fiyatını sisteme ekler.)
Ana proje çalışıyor olmasına rağmen GetStockService'in post metodu hata verir ise baseUrl'i güncelleyin.
örnek: string BaseUrl = "https://localhost:7261/api/Stock/SetStocks";

- Bir solution'da iki adet proje olduğu için add-migration , update-database kimi komutları çalıştırırken hata alabilirsiniz.
Bu hatayı engellemek için komutarı çalıştırırken projeyi belirtin. Örnek: Update-Database -Project StocksMarketWebAPI , add-migration -Project StocksMarketWebAPI migrationName

- Projede Bearer Token kullanılmaktır, login olduktan sonra backend tokeni geri döndürür.

- Bazı metotlar için admin yetkisi gerekir. Admin hesabını kullanmak için - username: admin password: admin
**