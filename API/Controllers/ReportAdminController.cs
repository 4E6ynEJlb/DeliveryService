using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("reports")]
    [ApiController]
    [Authorize("RequireAdminRole")]
    public class ReportAdminController : ControllerBase
    {
        private readonly IReportAdminService _reportAdminService;
        public ReportAdminController(IReportAdminService reportAdminService)
        {
            _reportAdminService = reportAdminService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetReport(DateTime id, CancellationToken cancellationToken)
        {
            if (id == DateTime.MinValue)
                return BadRequest("Arguments are null");
            return Ok(await _reportAdminService.GetReportByIdAsync(id, cancellationToken));
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetReportKeys(CancellationToken cancellationToken)
        {
            return Ok(await _reportAdminService.GetReportsIdsAsync(cancellationToken));
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> CloseReport(DateTime id, CancellationToken cancellationToken)
        {
            if (id == DateTime.MinValue)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _reportAdminService.RemoveReportAsync(id, userGuid, cancellationToken);
            return Ok();
        }
    }
}
