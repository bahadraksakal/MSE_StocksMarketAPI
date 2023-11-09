<h1 align="center"> Mert Software - Long Term Imep Internship - StockMarket Web API Application </h1>

![Mert Yazlım - Uzun Dönem Imep Staj - StockMarket Web API Uygulaması_1](/img/35.png)
![Mert Yazlım - Uzun Dönem Imep Staj - StockMarket Web API Uygulaması_2](/img/35_2.png)
![Mert Yazlım - Uzun Dönem Imep Staj - StockMarket Web API Uygulaması_3](/img/35_3.png)

****

- [EN : Description :book: :leftwards_arrow_with_hook:](#en)  
- [TR : Açıklama :book: :leftwards_arrow_with_hook:](#tr)

****

#### [EN]

- Created by **Ahmet Bahadır Aksakal** [@bahadraksakal](https://github.com/bahadraksakal).

## Problem Definition

Our project goal is to create a Web API service of a platform where stock portfolios are managed by the user in the admin role and registered users can perform purchase and sale transactions. It is not required to create an interface for the project. It is enough to create a collection on Postman and organize the requests in the required folder structure. Add test scripts to Postman requests according to "HTTP Status Code" returns. The stock information in the project should be updated regularly via a live Web API. The service that updates the stock information should be designed as a separate consol application and run once a minute by the Background Job application. Background Job should run as a separate consol project.\
:page_with_curl: **[Click for Project Details](https://github.com/bahadraksakal/MSE_StocksMarketAPI/blob/main/Stock_Market_DOTNET_Projesi.pdf)**

## Project Aims and Objectives

The aim of this project is to develop a StockMarket Web API application in accordance with enterprise software development processes, clean code writing principles and best practices.

## Project Features

- The user with the admin role and the MainBoard that holds my main site information are created at startup.
- Admin can create new users and assign admin or user role to the users he/she creates.
- Users should be able to register to the system and should be assigned to the user role by default.
- When a user with the user role is created, a portfolio is created by the system in which the stock information of the user will be kept.
- There is a balance service that contains user and system balance information.
- Stocks are initially created by the API from a different source. The number of stocks is initially set to 10,000 by default.
- Stock prices are regularly updated from the source used and users are allowed to trade with the last price information of the stock.
- Two digit value is used for decimal precision in stock pricing.
- Stock price movements can be kept in a table and historical price change information can be queried.
- Admin can end or start buying and selling transactions by stopping the activation of stocks.
- Admin can define balance card codes and balance amount based on balance codes for users to buy and sell. Each balance card can be used only once.
- Users can load balances by sending requests with these balance card codes.
- Users can purchase shares for their own portfolios as long as their balance amounts and the number of shares in the system are sufficient.
- Users can perform sales transactions as long as the number of shares in their portfolios is sufficient.
- By default, a 5% commission is charged on purchase and sale transactions and entered into the system balance.
- System and user balance amounts and number of stocks are updated according to the purchase and sale information.
- Buying and selling transactions are kept in a table and the transactions of the user can be analyzed in date range.
- Users can create rule sets by setting lower or upper amount limits on stocks. They can be notified by e-mail when the stock price is updated.
- Users can print stock price and trading transactions as Excel files.
- Users can print stock price and trading transactions as Pdf files.
- At the end of each month, transaction summaries for that month can be sent to users in pdf and excel format.

## Technology and Languages Used in Projects

- Asp.Net Core 7 🧩
- .Net 7 (Console) 🧩
- C# 🧩
- MSSQL 🧩
- Postman (Test Script) 🧩
- JavaScript 🧩

## Packages Used in the Project

### GetStockServiceApp

- .NET 7 (Console)
- Microsoft.AspNetCore.Authentication.JwtBearer | Version="7.0.13"
- Microsoft.Extensions.Configuration | Version="7.0.0"
- Microsoft.Extensions.Configuration.Json | Version="7.0.0.0"
- Serilog | Version="3.0.1"
- Serilog.Sinks.Console | Version="4.1.0"
- Serilog.Sinks.File | Version="5.0.0"

### HangFireApp

- .NET 7 (Console)
- Hangfire | Version="1.8.6"
- Hangfire.SqlServer | Version="5.1.2"
- Microsoft.Data.SqlClient Version="5.1.2"
- Microsoft.Extensions.Configuration | Version="7.0.0"
- Microsoft.EntityFrameworkCore" Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.13"
- Microsoft SQL Server | Version="15.0.4153"
- Serilog | Version="3.0.1"
- Serilog.Sinks.Console | Version="4.1.0"
- Serilog.Sinks.File | Version="5.0.0"

#### StocksMarketWebAPI

- Asp.NET Core 7
- AutoMapper.Extensions.Microsoft.DependencyInjection | Version="12.0.1"
- Microsoft.AspNetCore.Authentication.JwtBearer | Version="7.0.13"
- Microsoft.AspNetCore.OpenApi | Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer | Version="7.0.13"
- Microsoft.EntityFrameworkCore.Tools | Version="7.0.13"
- Serilog.AspNetCore | Version="7.0.0"
- Serilog.Sinks.MSSqlServer | Version="6.3.0"
- Swashbuckle.AspNetCore | Version="6.5.0"

### HangFireDbContextLibrary

- Microsoft.EntityFrameworkCore | Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer | Version="7.0.13"
- Microsoft.EntityFrameworkCore.Tools | Version="7.0.13"

### SharedServices

- EPPlus | Version="7.0.0"
- iTextSharp.LGPLv2.Core | Version="3.4.12"
- Serilog.AspNetCore | Version="7.0.0"
- Serilog.Sinks.Debug | Version="2.0.0"
- Serilog.Sinks.File | Version="5.0.0.0" 

### StocksMarketDbContextLibrary

- Microsoft.EntityFrameworkCore | Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer | Version="7.0.13"
- Microsoft.EntityFrameworkCore.Tools | Version="7.0.13"

#### StocksMarketEntitiesLibrary

- Microsoft.EntityFrameworkCore | Version="7.0.13"

### ModalsLibrary

- None

## Project Stand Up Steps, General Project Logic and Notes

### General Working Logic of the Project

- Solition contains 3 applications and libraries used by these applications. GetStockService, HangFireApp and StocksMarketWebAPI applications form the basic solition content.
- StocksMarketWebAPI and HangFireApp are configured to stand up and run together.
- HangFireApp runs the GetStockService application via its own method.
- HangFireApp application finds the stocks whose price changes and sends an email to the users who own and follow these stocks.
- At the end of each month, HangFireApp sends transaction summary reports to users in excel and pdf format.
- In order to detect stocks whose prices change, a matching process is performed with a second database, thus distributing the system load.
- GetStockService is defined as a dependency of HangFireApp, so the GetStockService application is automatically compiled as the Web API and HangFireApp are launched.
- HangFireApp runs GetStockService once every 3 minutes.
- HangFireApp runs stock tracking and email service 1 time in 3 minutes.
- HangFireApp runs SendMailMonthlyReport job 1 time per month.
- GetStockService will pull all stocks and send a request to StocksMarketWebAPI (security check with token)
- StocksMarketWebAPI application will automatically process the shares received through the controller and save them to the database. (If there is no stock, it adds a new record, if there is a stock, it adds the last price to the system.)
- You can get your trading history or price movements of the relevant stock as excel output via StockMarketWeb API.

### Project Stand Up Steps

1. Clone the project.

2. Install the project's dependencies using NuGet Package Manager to install the required packages before running the project.

3. To run the project as Default:\
 &emsp a) Run the project.
   
4. To run the project Manually:\
 &emsp a) Run the StocksMarketWebAPI application.\
 &emsp b) Then compile the GetStockService application.\
 &emsp c) Run the HangFireApp application.

5. HangFireApp runs GetStockService once per minute. GetStockService pulls all the stocks and sends this data to the Web API.

6. The StocksMarketWebAPI application automatically processes the stocks it receives and saves them in the database (if the stock does not exist, it adds a new record; if it exists, it updates its last price).

### Notes

- If the PUT method of GetStockService throws an error even though the main part of the project works, update the `BaseUrl` value in the `appconfig` file. Example: `"BaseUrl": "https://localhost:7261/api/Stock/SetStocks"`

- To test the project, import a Postman generated and exported collection file (compatible with Postman V2.1). Update Token and BaseUrl in Postman environmental variables (Env.) according to your test scenario to test the project.

- Review the configuration files in all three projects for database issues and adjust the Connection String according to your environment if necessary.

- Since there are three different projects in a solution, you may get errors while running `add-migration`, `update-database` etc. commands. To avoid these errors, you must specify the target project when running the command. Example: `Update-Database -Project StocksMarketWebAPI`, `add-migration -Project StocksMarketWebAPI migrationName`

- If you get an error that there are two context objects, apply a typing method like `add-migration -Context ContextName` etc.

- Some operations may require administrator authorization. Administrator account: { username: admin, password: admin }.


## [Click for Database Design :point_left:](#Veritabanı-Tasarımı)

## [Click for Images from the Program :point_left:](#Programdan-Görseller)



****
****



<h1 align="center"> Mert Yazlım - Uzun Dönem Imep Staj - StockMarket Web API Uygulaması </h1>


#### [TR]

- **Ahmet Bahadır Aksakal** [@bahadraksakal](https://github.com/bahadraksakal) tarafından oluşturuldu.

## Problem Tanımı

Proje hedefimiz admin rolündeki kullanıcı tarafından hisse senedi portföylerinin yönetildiği kayıtlı kullanıcıların alım, satım işlemleri gerçekleştirebildiği bir platformun Web API servisini oluşturmak. Proje için bir arayüz oluşturulması istenmemektedir. Postman üzerinde bir collection oluşturarak istekleri gerekli klasör yapısında düzenlemeniz yeterlidir. Postman isteklerine “HTTP Status Code” dönüşlerine göre test scriptlerini ekleyiniz. Projedeki hisse senedi bilgileri canlı olarak bir Web API üzerinden düzenli olarak güncellensin. Hisse senedi bilgilerini güncelleyen servis ayrı bir consol uygulaması olarak tasarlansın ve Background Job uygulaması tarafından dakikada 1 kez çalıştırılsın. Background Job'lar ayrı bir consol projesi olarak çalışmalıdır.\
:page_with_curl: **[Proje Detayları İçin Tıkla](https://github.com/bahadraksakal/MSE_StocksMarketAPI/blob/main/Stock_Market_DOTNET_Projesi.pdf)**


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
- Kullanıcılar tarafından hisse senedi fiyat ve alım satım hareketleri Pdf dosyası olarak çıktı alınabilir.
- Kullancılara her ay sonunda, o aya ait işlem özetleri pdf ve excel formatında gönderilebilir.

## Projelerde Kullanılan Teknoloji Ve Diller

- Asp.Net Core 7 🧩
- .Net 7 (Console) 🧩
- C# 🧩
- MSSQL 🧩
- Postman (Test Script) 🧩
- JavaScript 🧩

## Projede Kullanılan Paketler

### GetStockServiceApp

- .NET 7 (Console)
- Microsoft.AspNetCore.Authentication.JwtBearer | Version="7.0.13"
- Microsoft.Extensions.Configuration | Version="7.0.0"
- Microsoft.Extensions.Configuration.Json | Version="7.0.0"
- Serilog | Version="3.0.1"
- Serilog.Sinks.Console | Version="4.1.0"
- Serilog.Sinks.File | Version="5.0.0"

### HangFireApp

- .NET 7 (Console)
- Hangfire | Version="1.8.6"
- Hangfire.SqlServer | Version="5.1.2"
- Microsoft.Data.SqlClient Version="5.1.2"
- Microsoft.Extensions.Configuration | Version="7.0.0"
- Microsoft.EntityFrameworkCore" Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.13"
- Microsoft SQL Server | Version="15.0.4153"
- Serilog | Version="3.0.1"
- Serilog.Sinks.Console | Version="4.1.0"
- Serilog.Sinks.File | Version="5.0.0"

### StocksMarketWebAPI

- Asp.NET Core 7
- AutoMapper.Extensions.Microsoft.DependencyInjection | Version="12.0.1"
- Microsoft.AspNetCore.Authentication.JwtBearer | Version="7.0.13"
- Microsoft.AspNetCore.OpenApi | Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer | Version="7.0.13"
- Microsoft.EntityFrameworkCore.Tools | Version="7.0.13"
- Serilog.AspNetCore | Version="7.0.0"
- Serilog.Sinks.MSSqlServer | Version="6.3.0"
- Swashbuckle.AspNetCore | Version="6.5.0"

### HangFireDbContextLibrary

- Microsoft.EntityFrameworkCore | Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer | Version="7.0.13"
- Microsoft.EntityFrameworkCore.Tools | Version="7.0.13"

### SharedServices

- EPPlus |  Version="7.0.0"
- iTextSharp.LGPLv2.Core | Version="3.4.12"
- Serilog.AspNetCore | Version="7.0.0"
- Serilog.Sinks.Debug | Version="2.0.0"
- Serilog.Sinks.File | Version="5.0.0" 

### StocksMarketDbContextLibrary

- Microsoft.EntityFrameworkCore | Version="7.0.13"
- Microsoft.EntityFrameworkCore.SqlServer | Version="7.0.13"
- Microsoft.EntityFrameworkCore.Tools | Version="7.0.13"

### StocksMarketEntitiesLibrary

- Microsoft.EntityFrameworkCore | Version="7.0.13"

### ModalsLibrary

- None

## Proje Ayağa Kaldırma Adımları, Projenin Genel Çalışma Mantığı Ve Notlar

### Projenin Genel Çalışma Mantığı

- Solition içerisinde 3 adet uygulama ve bu uygulamalr tarafından kullanılan kütüphaneler bulunur. GetStockService ,HangFireApp ve StocksMarketWebAPI uygulamaları temel solition içeriğini oluşturur.
- StocksMarketWebAPI ve HangFireApp uygulaması birlikte ayağa kalkıp, çalışacak şekilde konfigüre edilmiştir.
- HangFireApp uygulaması kendi metodu üzerinden GetStockService uygulamasını çalıştırır.
- HangFireApp uygulaması kendi metodu üzerinden fiyatı değişen hisse senetlerini bulup, bu hisse senetlerine sahip olan ve bu hisse senetlerini takip eden kullanıcılara email atar.
- HangFireApp uygulaması her ay sonu kullanıcılara işlem özet rapolarını excel ve pdf formatında email olarak gönderir.
- Fiyatı değşen hisse senetlerinin tespiti için ikinci bir veri tabanı ile eşleştirme işlemi yapılır bu sayede sistem yükü dağıtılmış olur.
- GetStockService, HangFireApp'in bağımlılığı olarak tanımlanmıştır, bu sayede Web API ve HangFireApp ayağa kalkarken GetStockService uygulamasıda otomatik olarak derlenir.
- HangFireApp 3 dakikada 1 kez GetStockService uygulamasını çalıştırır.
- HangFireApp 3 dakikada 1 kez hisse senedi takip ve email servisini çalıştırır.
- HangFireApp ayda 1 kez SendMailMonthlyReport işini çalıştırır.
- GetStockService tüm hisseleri çekip, StocksMarketWebAPI uygulamasına istek atıcaktır. (Token ile güvenlik kontrolü)
- StocksMarketWebAPI uygulaması controller üzerinden aldığı hisseleri otomatik olarak işleyip, veritabanına kayıt edicektir. (Hisse senedi yoksa yeni kayıt ekler, var ise son fiyatını sisteme ekler.)
- StockMarketWeb API üzerinden alım satım geçmişinizi veya ilgili hisse senedine ait fiyat hareketlerini excel çıktısı olarak alabilirsiniz.

### Proje Ayağa Kaldırma Adımları

1. Projeyi klonlayın.

2. Projeyi çalıştırmadan önce gerekli paketleri yüklemek için NuGet Paket Yöneticisi'ni kullanarak projenin bağımlılıklarını yükleyin.

3. Projeyi Default olarak çalıştırmak için:\
 &emsp a) Projeyi çalıştırın.

4. Projeyi Manuel olarak çalıştırmak için:\
 &emsp a) StocksMarketWebAPI uygulamasını çalıştırın.\
 &emsp b) Ardından GetStockService uygulamasını derleyin.\
 &emsp c) HangFireApp uygulamasını çalıştırın.

4. HangFireApp, dakikada bir kez GetStockService uygulamasını çalıştırır. GetStockService, tüm hisseleri çeker ve bu verileri Web API'ye gönderir.

5. StocksMarketWebAPI uygulaması, aldığı hisseleri otomatik olarak işler ve veritabanına kaydeder (eğer hisse senedi yoksa, yeni kayıt ekler; varsa, son fiyatını günceller).

### Notlar

- Projenin ana parçası çalışsa da, GetStockService uygulamasının PUT yöntemi bir hata verirse, `appconfig` dosyasındaki `BaseUrl` değerini güncelleyin. Örnek: `"BaseUrl":"https://localhost:7261/api/Stock/SetStocks"`

- Projeyi test etmek için, Postman tarafından oluşturulmuş ve dışa aktarılmış bir koleksiyon dosyasını (Postman V2.1 sürümüyle uyumlu) içe aktarın. Projeyi test etmek için Postman çevresel değişkenler (Env.) içindeki Token ve BaseUrl'i test senaryonuza göre güncelleyin.

- Veritabanı sorunları için her üç projedeki yapılandırma dosyalarını gözden geçirin ve gerektiğinde Connection String'i kendi ortamınıza göre ayarlayın.

- Bir çözüm içerisinde üç farklı proje olduğu için `add-migration`, `update-database` vb. komutlarını çalıştırırken hatalar alabilirsiniz. Bu hataları önlemek için komutu çalıştırırken hedef projeyi belirtmelisiniz. Örnek: `Update-Database -Project StocksMarketWebAPI`, `add-migration -Project StocksMarketWebAPI migrationName`

- İki adet context nesnesi olduğuna ilişkin bir hata alırsanız,  `add-migration -Context ContextName` vb.. tarzında bir yazım metodu uygulayın.

- Bazı işlemler için yönetici yetkisi gerekebilir. Yönetici hesabı: { kullanıcı adı: admin, şifre: admin }.

## Veritabanı Tasarımı

![dbdiagram](/img/dbdiagram.png)

## Programdan Görseller

![Resim 34](/img/37.png)
![Resim 34](/img/36.png)
![Resim 34](/img/38.png)
![Resim 34](/img/35.png)
![Resim 34](/img/34.png)
![Resim 31](/img/31.png)
![Resim 33](/img/33.png)
![Resim 32](/img/32.png)
![Resim 30](/img/30.png)
![Resim 35](/img/35.png)
![Resim 35_2](/img/35_2.png)
![Resim 35_3](/img/35_3.png)
![Resim 1](/img/1.png)
![Resim 1_2](/img/1_2.png)
![Resim 2](/img/2.png)
![Resim 3](/img/3.png)
![Resim 4](/img/4.png)
![Resim 5](/img/5.png)
![Resim 6](/img/6.png)
![Resim 7](/img/7.png)
![Resim 8](/img/8.png)
![Resim 9](/img/9.png)
![Resim 10](/img/10.png)
![Resim 11](/img/11.png)
![Resim 12](/img/12.png)
![Resim 13](/img/13.png)
![Resim 14](/img/14.png)
![Resim 15](/img/15.png)
![Resim 16](/img/16.png)
![Resim 17](/img/17.png)
![Resim 18](/img/18.png)
![Resim 19](/img/19.png)
![Resim 20](/img/20.png)
![Resim 21](/img/21.png)
![Resim 22](/img/22.png)
![Resim 23](/img/23.png)
![Resim 24](/img/24.png)
![Resim 25](/img/25.png)
![Resim 26](/img/26.png)
![Resim 27](/img/27.png)
![Resim 28](/img/28.png)
![Resim 29](/img/29.png)
