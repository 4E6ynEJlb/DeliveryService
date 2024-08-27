using Application.Interfaces;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("restaurants")]
    [ApiController]
    [Authorize("RequireAdminRole")]
    public class RestaurantAdminController : ControllerBase
    {
        private readonly IRestaurantAdminService _restaurantAdminService;
        public RestaurantAdminController(IRestaurantAdminService restaurantAdminService) 
        {
            _restaurantAdminService = restaurantAdminService;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add(Restaurant restaurant, CancellationToken cancellationToken)
        {
            if (restaurant == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _restaurantAdminService.AddRestaurantAsync(restaurant, userGuid, cancellationToken);
            return Ok();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Remove(string adress, CancellationToken cancellationToken)
        {
            if (adress == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _restaurantAdminService.RemoveRestaurantAsync(adress, userGuid, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> EditAuth(string adress, AuthModel authModel, CancellationToken cancellationToken)
        {
            if (adress == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _restaurantAdminService.EditRestaurantAuthAsync(adress, authModel, userGuid, cancellationToken);
            return Ok();
        }
    }
}
