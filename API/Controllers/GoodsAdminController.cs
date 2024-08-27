using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("goods")]
    [ApiController]
    [Authorize("RequireAdminRole")]
    public class GoodsAdminController : ControllerBase
    {
        private readonly IGoodsAdminService _goodsAdminService;
        public GoodsAdminController(IGoodsAdminService goodsAdminService) 
        {
            _goodsAdminService = goodsAdminService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> InvisibleList(int page, int pageSize, string? textInTitle, CancellationToken cancellationToken)
        {
            return Ok(await _goodsAdminService.GetInvisibleGoodsArrayAsync(page, pageSize, textInTitle, cancellationToken));
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add(ProductInputModel product, CancellationToken cancellationToken)
        {
            if (product == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.AddProductAsync(product, userGuid, cancellationToken);
            return Ok();
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> Remove(int article, CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.RemoveProductAsync(article, userGuid, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> EditPrice(int article, decimal price, CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.EditPriceAsync(article, price, userGuid, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> Show(int article, CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.ShowProductAsync(article, userGuid, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> Hide(int article, CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.HideProductAsync(article, userGuid, cancellationToken);
            return Ok();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AttachImage(IFormFile file, int article, CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.AttachImageAsync(file, article, userGuid, cancellationToken);
            return Ok();
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DetachImage(int article, CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.DetachImageAsync(article, userGuid, cancellationToken);
            return Ok();
        }
    }
}
