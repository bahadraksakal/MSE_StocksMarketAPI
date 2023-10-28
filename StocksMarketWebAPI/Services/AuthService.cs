using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.DTOs.UserDTOs;
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
        private readonly IMapper _mapper;

        public AuthService(StockMarketDbContext stockMarketDbContext, IConfiguration configuration,IMapper mapper)
        {
            _stockMarketDbContext = stockMarketDbContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserDTO> CheckCredentialsAsync(string userName, string userPassword)
        {            
            User user = await _stockMarketDbContext.Users.FirstOrDefaultAsync(user => user.Name == userName && user.Password == userPassword);
            if (user != null)
            {
                Log.Warning($"AuthService-CheckCredentialsAsync: {userName} giriş yaptı.");
                return _mapper.Map<UserDTO>(user);
            }
            Log.Warning($"AuthService-CheckCredentialsAsync: {userName} hatalı giriş denemesi yaptı.");
            return null;
        }

        public string CreateToken(UserDTO user)
        {
            
            Claim[] Claims = new[]
                    {
                        new Claim("userName", user.Name),
                        new Claim("userId", user.Id.ToString()), 
                        new Claim(ClaimTypes.Role, user.Role),
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
