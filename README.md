# MSE_StocksMarketAPI
asp.net core projesi 

- Genel çalışma prensibi: Solition'da 3 adet proje bulunur. Web API ve HangFireApp birlikte ayağa kalkıp çalışacak şekilde konfügre edilmiştir.
 HangFireApp kendi metodu üzerinden GetStockService uygulamasını çalıştırır. GetStockService, HangFireApp'in bağımlılığı olarak tanımlanmıştır bu sayede 
 Web API ve HangFireApp ayağa kalkarken GetStockService uygulmasıda derlenir.
 Eğer proje çalışma konfigrasyonunu kullanmadan manuel kullanmak için: 
		Hisse senetlerini çekmek için: Önce ana Web API projesini çalıştırın. Daha sonra GetStockService projesini derleyin. 
	Bu işlemden sonra HangFireApp uygulamasını çalıştırın. HangFireApp dakikada 1 kez GetStockService uygulamasını çalıştırır.
	GetStockService tüm hisseleri çekip, Web API ye istek atıcaktır. Web API hisseleri otomatik olarak veritabanına kayıt edicektir.
	(Hisse senedi yoksa yeni kayıt ekler,, var ise son fiyatını sisteme ekler.)
	Ana proje çalışıyor olmasına rağmen GetStockService'in put metodu hata verir ise BaseUrl'i appconfig dosayası içinden güncelleyin.
	Örnek: "BaseUrl":"https://localhost:7261/api/Stock/SetStocks"

- Projeyi test etmek için, export edilmiş postman dosyasını import edin - Postman V2.1
Postman üzerinden projeyi test edebilirsiniz. Testlerinize göre Postman Env. içerisindeki Tokeni ve BaseUrl'i güncellemeyi unutmayın

- Database sorunları için 3 projedeki config dosyalarınıda inceleyin. Gerekirse ConnectionStringleri kendinize göre değiştirin.

- Bir solution'da 3 adet proje olduğu için add-migration , update-database kimi komutları çalıştırırken hata alabilirsiniz.
Bu hatayı engellemek için komutarı çalıştırırken projeyi belirtin.
Örnek: Update-Database -Project StocksMarketWebAPI , add-migration -Project StocksMarketWebAPI migrationName

- Projede Bearer Token kullanılmaktır, login olduktan sonra backend tokeni geri döndürür.

- Bazı metotlar için admin yetkisi gerekir. Admin hesabını kullanmak için - username: admin password: admin
