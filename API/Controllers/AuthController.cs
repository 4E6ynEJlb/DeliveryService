using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITelegramBotApi _telegramBotApi;
        public AuthController(IAuthService authService, ITelegramBotApi telegramBotApi)
        {
            _authService = authService;
            _telegramBotApi = telegramBotApi;
        }      
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AuthHost(AuthModel authModel, CancellationToken cancellationToken)
        {
            if (authModel == null)
                return BadRequest("Arguments are null");
            if (!_telegramBotApi.IsHostLogined)
            {
                await _telegramBotApi.SendHostAuthMessageAsync(cancellationToken);
                return Unauthorized(); 
            }
            if(!_authService.AuthHost(authModel))
                return BadRequest("Incorrect login and/or password");
            string role = "HostRole";
            DateTime utcNow = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Client,
                    notBefore: utcNow,
                    claims: GetIdentity("Host", role).Claims,
                    expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(encodedJwt);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AuthRestaurant(AuthModel authModel, CancellationToken cancellationToken)
        {
            if (authModel == null)
                return BadRequest("Arguments are null");
            Restaurant restaurant = await _authService.GetRestaurantByAuthAsync(authModel, cancellationToken);
            string role = "RestaurantRole";
            DateTime utcNow = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Client,
                    notBefore: utcNow,
                    claims: GetIdentity(restaurant.Adress, role).Claims,
                    expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(encodedJwt);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AuthUser(AuthModel authModel, CancellationToken cancellationToken)
        {
            if (authModel == null)
                return BadRequest("Arguments are null");
            UserOutputModel user = await _authService.GetUserByAuthAsync(authModel, cancellationToken);
            string role = "UserRole";
            if (user.IsAdmin)
            {
                role = "AdminRole";
            }
            DateTime utcNow = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Client,
                    notBefore: utcNow,
                    claims: GetIdentity(user.Id.ToString(), role).Claims,
                    expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(encodedJwt);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterModel userRegisterModel, CancellationToken cancellationToken)
        {
            if (userRegisterModel == null)
                return BadRequest("Arguments are null");
            await _authService.RegisterUserAsync(userRegisterModel, cancellationToken);
            return Ok();
        }
        private ClaimsIdentity GetIdentity(string id, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, id),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
