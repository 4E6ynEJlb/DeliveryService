using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("auditlog")]
    [Authorize("RequireHostRole")]
    public class AuditLogHostController : ControllerBase
    {
        private readonly IAuditHostService _auditHostService;
        public AuditLogHostController(IAuditHostService auditHostService)
        {
            _auditHostService = auditHostService;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
        {            
            return Ok(await _auditHostService.GetRecordsCountAsync(cancellationToken));
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Getlist(int count, CancellationToken cancellationToken)
        {
            return Ok(await _auditHostService.GetLastRecordsAsync(count, cancellationToken));
        }
    }
}
