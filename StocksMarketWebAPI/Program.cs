using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksMarketWebAPI.Context;
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
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr"));
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MoneyCardService>();
builder.Services.AddScoped<MainBoardService>();
builder.Services.AddScoped<StockService>();

//seri log
builder.Services.AddSerilog();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Debug(Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File("logs/log.txt",Serilog.Events.LogEventLevel.Warning)
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

app.UseHttpsRedirection();

app.UseAuthentication();//önce
app.UseAuthorization();

app.MapControllers();

app.Run();
