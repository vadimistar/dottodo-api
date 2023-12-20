using dottodo.Common;
using dottodo.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dottodo.Auth
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;
        private IUserRepository _userRepository;

        public TokenService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public string CreateAccessToken(string userId, string username)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new(Constants.ClaimId, userId),
                        new(ClaimTypes.Name, username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public TokenDto RefreshToken(string oldToken)
        {
            var user = _userRepository.GetByRefreshToken(oldToken); 

            if (user == null)
            {
                throw new TokenServiceException("invalid refresh token");
            }

            if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
            {
                throw new TokenServiceException("expired token");
            }

            var refreshToken = UpdateRefreshToken(user.Id)?.RefreshToken;

            var accessToken = CreateAccessToken(user.Id, user.UserName);

            return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public RefreshTokenDto UpdateRefreshToken(string userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null) { throw new TokenServiceException("user is invalid"); }

            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = refreshTokenExpiresAt;

            _userRepository.SaveChanges();

            return new RefreshTokenDto { RefreshToken = refreshToken, ExpiresAt = refreshTokenExpiresAt };
        }
    }
}
