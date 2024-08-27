using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize("RequireHostRole")]
    public class UserHostController : ControllerBase
    {
        private readonly IUserHostService _userHostService;
        public UserHostController(IUserHostService userHostService) 
        {
            _userHostService = userHostService;
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> DebitBonuses(Guid id, decimal amount, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _userHostService.DebitBonusesAsync(id, amount, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> AssignAdm(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _userHostService.AssignUserAsAdminAsync(id, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> UnassignAdm(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _userHostService.UnassignUserAsAdminAsync(id, cancellationToken);
            return Ok();
        }
    }
}
