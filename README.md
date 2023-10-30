# StockMarketAPI

![Uzun Dönem Imep Staj StockMarket Web API Uygulaması](/img/cover-image.png)

## Problem Tanımı

Proje hedefimiz admin rolündeki kullanıcı tarafından hisse senedi portföylerinin yönetildiği kayıtlı kullanıcıların alım, satım işlemleri gerçekleştirebildiği bir platformun Web API servisini oluşturmak. Proje için bir arayüz oluşturulması istenmemektedir. Postman üzerinde bir collection oluşturarak istekleri gerekli klasör yapısında düzenlemeniz yeterlidir. Postman isteklerine “HTTP Status Code” dönüşlerine göre test scriptlerini ekleyiniz. Projedeki hisse senedi bilgileri canlı olarak bir Web API üzerinden düzenli olarak güncellensin. Hisse senedi bilgilerini güncelleyen servis ayrı bir consol uygulaması olarak tasarlansın ve Background Job uygulaması tarafından dakikada 1 kez çalıştırılsın. Background Job'lar ayrı bir consol projesi olarak çalışmalıdır.

## Proje Amacı ve Hedefleri

Bu projenin amacı, kurumsal yazılım geliştirme süreçleri, temiz kod yazma prensipleri ve best practices'e uygun bir StockMarket Web API uygulaması geliştirmektir.

## Proje Özellikleri

- Admin rolüne sahip kullanıcı ve Ana sitem bilgilerini tutan MainBoard başlangıçta oluşturulur.
- Admin yeni kullanıcılar oluşturabilir ve oluşturduğu kullanıcılara admin veya user rolü atayabilir.
- Kullanıcılar sisteme kayıt olabilmeli ve varsayılan olarak user rolüne atanmalıdır.
- User rolüne sahip kullanıcı oluştuğunda kullanıcıya ait hisse senetleri bilgisinin tutulacağı bir portföy sistem tarafından oluşturulur.
- Kullanıcı ve sistem bakiye bilgisini içeren bir bakiye servisi bulunur.
- Hisse senetleri farklı bir kaynaktan alınarak API tarafından başlangıçta oluşturulur. Hisse senedi adet bilgisini başlangıçta varsayılan olarak 10,000 adet olarak ayarlanır.
- Hisse senedi fiyatları düzenli olarak kullanılan kaynaktan güncellenir ve kullanıcılara hissenin son fiyat bilgisi ile işlem yaptırılır.
- Hisse fiyatlandırmalarında ondalık hassasiyet için iki haneli değer kullanılır.
- Hisse senedi fiyat hareketleri bir tabloda tutularak geçmişe yönelik fiyat değişim bilgisi sorgulanabilir.
- Admin hisse senetlerinin aktifliğini durdurarak alım ve satım işlemlerini sonlandırabilir veya başlatabilir.
- Admin kullanıcıların alım satım yapabilmesi için bakiye kart kodları ve bakiye kodlarına bağlı bakiye miktarı tanımlayabilir. Her bakiye kartı sadece bir kez kullanılabilir.
- Kullanıcılar bu bakiye kart kodları ile istek atarak kendilerine bakiye yükleyebilir.
- Kullanıcılar bakiye miktarları ve sistem tarafındaki hisse adetleri yeterli olduğu sürece kendi portföylerine hisse alımı gerçekleştirebilir.
- Kullanıcılar portföylerindeki hisse senedi adeti yeterli olduğu sürece satış işlemi gerçekleştirebilir.
- Alım ve satım işlemleri üzerinden varsayılan olarak %5 komisyon alınarak sistem bakiyesine işlenir.
- Sistem ve kullanıcı bakiye miktarları ve hisse senedi adetleri alım satım bilgisine göre güncellenir.
- Alım, satım hareketleri bir tabloda tutularak kullanıcıya ait hareketler tarih aralığında incelenebilir.
- Kullanıcılar hisse senetlerinde alt veya üst miktar limiti belirleyerek kural setleri oluşturabilir. Hisse senedi fiyatı güncellendiğinde e-posta ile bilgilendirme alabilirler.
- Kullanıcılar tarafından hisse senedi fiyat ve alım satım hareketleri Excel dosyası olarak çıktı alınabilir.

## Projede Kullanılan Teknoloji ve Diller

### GetStockServiceApp

- .NET 7 (Console)
- Microsoft.AspNetCore.Authentication.JwtBearer | Version="7.0.13"
- Microsoft.Extensions.Configuration | Version="7.0.0"
- Microsoft.Extensions.Configuration.Json | Version="7.0.0"

### HangFireApp

- .NET 7 (Console)
- Hangfire | Version="1.8.6"
- Hangfire.SqlServer | Version="5.1.2"
- Microsoft.Data.SqlClient Version="5.1.2"
- Microsoft.Extensions.Configuration | Version="7.0.0"
- Microsoft SQL Server | Version="15.0.4153"

### StocksMarketWebAPI

- .NET 7
- AutoMapper.Extensions.Microsoft.DependencyInjection | Version="12.0.1"
- Microsoft.AspNetCore.Authentication.JwtBearer | Version="7.0.12"
- Microsoft.AspNetCore.OpenApi | Version="7.0.12"
- Microsoft.EntityFrameworkCore.SqlServer | Version="7.0.12"
- Microsoft.EntityFrameworkCore.Tools | Version="7.0.12"
- Serilog.AspNetCore | Version="7.0.0"
- Serilog.Sinks.MSSqlServer | Version="6.3.0"
- Swashbuckle.AspNetCore | Version="6.5.0"
- Microsoft SQL Server | Version="15.0.4153"

## Proje Ayağa Kaldırma Adımları

1. Projeyi klonlayın.

2. Projeyi çalıştırmadan önce gerekli paketleri yüklemek için NuGet Paket Yöneticisi'ni kullanarak projenin bağımlılıklarını yükleyin.

3. Projeyi çalıştırın:

		Default ayarlar için genel çalışma prensibi:
			- Solition içerisinde 3 adet uygulama bulunur. GetStockService ,HangFireApp ve StocksMarketWebAPI uygulamaları solition içeriğini oluşturur.
			StocksMarketWebAPI ve HangFireApp uygulaması birlikte ayağa kalkıp, çalışacak şekilde konfügre edilmiştir.
			HangFireApp uygulaması kendi metodu üzerinden GetStockService uygulamasını çalıştırır. GetStockService, HangFireApp'in bağımlılığı olarak tanımlanmıştır, bu sayede 
			Web API ve HangFireApp ayağa kalkarken GetStockService uygulamasıda otomatik olarak derlenir. HangFireApp dakikada 1 kez GetStockService uygulamasını çalıştırır. GetStockService tüm hisseleri çekip, Web API e istek atıcaktır.
		StocksMarketWebAPI uygulaması controller üzerinden aldığı hisseleri otomatik olarak işleyip, veritabanına kayıt edicektir. (Hisse senedi yoksa yeni kayıt ekler, var ise son fiyatını sisteme ekler.)
		
		Manuel olarak çalıştırmak için:
		   - Önce StocksMarketWebAPI uygulamasını çalıştırın.
		   - Ardından GetStockService uygulamasını derleyin. Bu adımdan sonra HangFireApp uygulamasını çalıştırın.

4. HangFireApp, dakikada bir kez GetStockService uygulamasını çalıştırır. GetStockService, tüm hisseleri çeker ve bu verileri Web API'ye gönderir.

5. StocksMarketWebAPI uygulaması, aldığı hisseleri otomatik olarak işler ve veritabanına kaydeder (eğer hisse senedi yoksa, yeni kayıt ekler; varsa, son fiyatını günceller).

6. Postman veya benzeri bir API test aracı kullanarak projeyi test edebilirsiniz. Testlerinize göre Postman çevresel değişkenler (Env.) içindeki Token ve BaseUrl'i güncellemeyi unutmayın.

7. Database konfigürasyonları ile ilgili sorunlar yaşarsanız, projedeki config dosyalarını inceleyin ve gerekirse Connection String'i kendi ortamınıza göre düzenleyin.

8. Projedeki 3 farklı proje olduğu için Migration işlemleri yaparken her proje için belirli bir proje belirtmeyi unutmayın (örneğin: `Update-Database -Project StocksMarketWebAPI`).

9. Bazı metotlar için admin yetkisi gerekebilir. Admin hesabı: { username: admin, password: admin }.

### Notlar

- Projenin ana parçası çalışsa da, GetStockService uygulamasının PUT yöntemi bir hata verirse, `appconfig` dosyasındaki `BaseUrl` değerini güncelleyin. Örnek: `"BaseUrl":"https://localhost:7261/api/Stock/SetStocks"`

- Projeyi test etmek için, Postman tarafından oluşturulmuş ve dışa aktarılmış bir koleksiyon dosyasını (Postman V2.1 sürümüyle uyumlu) içe aktarın. Projeyi test etmek için Postman çevresel değişkenler (Env.) içindeki Token ve BaseUrl'i test senaryonuza göre güncelleyin.

- Veritabanı sorunları için her üç projedeki yapılandırma dosyalarını gözden geçirin ve gerektiğinde Connection String'i kendi ortamınıza göre ayarlayın.

- Bir çözüm içerisinde üç farklı proje olduğu için `add-migration`, `update-database` vb. komutlarını çalıştırırken hatalar alabilirsiniz. Bu hataları önlemek için komutu çalıştırırken hedef projeyi belirtmelisiniz. Örnek: `Update-Database -Project StocksMarketWebAPI`, `add-migration -Project StocksMarketWebAPI migrationName`

- Bazı işlemler için yönetici yetkisi gerekebilir. Yönetici hesabı: { kullanıcı adı: admin, şifre: admin }.

## Programdan Görseller

- [Görüntüler burada bulunabilir](/img)