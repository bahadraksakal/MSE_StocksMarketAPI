using HangFireDbContextLibrary.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using SharedServices.ExcelServices;
using SharedServices.PdfServices;
using SharedServices.StockTrackingServices;
using StockMarketDbContextLibrary.Context;
using StocksMarketWebAPI.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<StockMarketDbContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionString:ConStr"]);
});

builder.Services.AddDbContext<HangFireDbContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionString:ConStrHangFire"]);
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MoneyCardService>();
builder.Services.AddScoped<MainBoardService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<StockTrackingService>();
builder.Services.AddScoped<ExportExcelService>();
builder.Services.AddScoped<ExportPdfService>();


//seri log
builder.Services.AddSerilog();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.Debug()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//JwtBearer
var issuer = builder.Configuration["JwtConfig:Issuer"];
var audience = builder.Configuration["JwtConfig:Audience"];
var signingKey = builder.Configuration["JwtConfig:SigningKey"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
    };
});
builder.Services.AddAuthorization();

//özel authorize ekleme
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomServerPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.Role, "Server");
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomAdminPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.Role, "Admin");

    });
});


//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));//IMapper

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.CreateScope())
{
    try
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<StockMarketDbContext>();
        await context.Database.MigrateAsync();
    }
    catch(Exception ex)
    {
        Log.Error("Veri tabaný güncellenemedi veya baþarýyla oluþturulamadý. Mesaj: "+ex.Message);
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();//önce
app.UseAuthorization();

app.MapControllers();

app.Run();
