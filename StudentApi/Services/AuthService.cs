using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentApi.Configurations;
using StudentApi.Models;
using StudentApi.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentApi.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtConfig _jwtConfig;

        public AuthService(UserRepository userRepository, IOptions<JwtConfig> jwtConfig)
        {
            _userRepository = userRepository;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || user.Password != password)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}