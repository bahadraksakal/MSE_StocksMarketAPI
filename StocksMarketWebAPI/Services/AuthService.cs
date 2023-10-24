using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StocksMarketWebAPI.Services
{
    public class AuthService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly IConfiguration _configuration;

        public AuthService(StockMarketDbContext stockMarketDbContext, IConfiguration configuration)
        {
            _stockMarketDbContext = stockMarketDbContext;
            _configuration = configuration;
        }

        public async Task<User> CheckCredentialsAsync(string userName, string userPassword)
        {            
            var user = await _stockMarketDbContext.Users.FirstOrDefaultAsync(user => user.Name == userName && user.Password == userPassword);
            if (user != null)
            {
                Log.Warning($"AuthService-CheckCredentialsAsync: {userName} giriş yapmayı deniyor.");
                return user;
            }
            Log.Warning($"AuthService-CheckCredentialsAsync: {userName} hatalı giriş denemesi yaptı.");
            return null;
        }

        public string CreateToken(User user)
        {
            var Claims = new[]
                    {
                    new Claim("userName", user.Name),
                    new Claim("userId", user.Id.ToString()),
                };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SigningKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _configuration["JwtConfig:Issuer"], //token sağlayıcısı kim
                    audience: _configuration["JwtConfig:Audience"],
                    claims: Claims, // tokeni doğruladığımda erişebileceğim gömülü değerler
                    expires: DateTime.Now.AddHours(1), // 1 saat sonra token yok olsun
                    notBefore: DateTime.Now, // token geçerliği hemen başlasın
                    signingCredentials: credentials
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            Log.Warning($"AuthService-CreateToken: Token oluşturuldu: {token}");
            return token;
        }
    }
}
