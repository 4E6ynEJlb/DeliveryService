using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize("RequireUserRole")]
    public class UserUserController : ControllerBase
    {
        private readonly IUserUserService _userUserService;
        public UserUserController(IUserUserService userUserService) 
        {
            _userUserService = userUserService;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            return Ok(await _userUserService.GetUserByIdAsync(userGuid, cancellationToken));
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.DeleteUserAsync(userGuid, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> EditTG(string telegram, CancellationToken cancellationToken)
        {
            if (telegram == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.EditUserTelegramAsync(userGuid, telegram, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> AddBirthDate(DateOnly birthDate, CancellationToken cancellationToken)
        {
            if (birthDate == DateOnly.MinValue)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.AddUserBirthDateAsync(userGuid, birthDate, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> EditAuth(AuthModel authModel, CancellationToken cancellationToken)
        {
            if (authModel == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.EditUserAuthAsync(userGuid, authModel, cancellationToken);
            return Ok();
        }
    }
}
