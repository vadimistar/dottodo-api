using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

namespace dottodo.Auth
{
    using Common;
    using User;
    
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IErrorTranslationService _errorTranslationService;
        
        public AuthController(UserManager<UserEntity> userManager, ITokenService tokenService, IErrorTranslationService errorTranslationService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _errorTranslationService = errorTranslationService;
        }

        public class RegisterParams
        {
            [Required]
            [StringLength(255, MinimumLength = 4, ErrorMessage = "Логин слишком короткий.")]
            public string Username { get; set; } = "";

            [Required]
            public string Password { get; set; } = "";
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromQuery] RegisterParams registerParams)
        {
            var user = new UserEntity { UserName = registerParams.Username };

            var result = await _userManager.CreateAsync(user, registerParams.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            
            var errors = result.Errors.Select(
                error => _errorTranslationService.TranslateError(error.Code));
            
            return BadRequest(new { Errors = errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return Unauthorized();
            }

            var refreshToken = _tokenService.UpdateRefreshToken(user.Id);
            
            CookieOptions cookieOptions = new CookieOptions() 
            { 
                HttpOnly = true,
                Expires = refreshToken.ExpiresAt, 
            };

            Response.Cookies.Append(Constants.RefreshToken, refreshToken.RefreshToken, cookieOptions);

            var accessToken = _tokenService.CreateAccessToken(user.Id, user.UserName);

            return Ok(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken.RefreshToken });
        }

        [HttpPost("refresh_token")]
        public IActionResult RefreshToken()
        {
            var cookie = Request.Cookies[Constants.RefreshToken];

            if (cookie == null)
            {
                return BadRequest("cookie " + Constants.RefreshToken + " is not found");
            }

            try
            {
                return Ok(_tokenService.RefreshToken(cookie));
            }
            catch (TokenServiceException)
            {
                return Unauthorized("please login again");
            }
        }
    }
}
