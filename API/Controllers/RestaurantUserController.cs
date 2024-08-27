using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("restaurants")]
    [ApiController]
    public class RestaurantUserController : ControllerBase
    {
        private readonly IRestaurantUserService _restaurantUserService;
        public RestaurantUserController(IRestaurantUserService restaurantUserService)
        {
            _restaurantUserService = restaurantUserService;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetRestaurants(string city, CancellationToken cancellationToken)
        {
            if (city == null)
                return BadRequest("Arguments are null");
            return Ok(await _restaurantUserService.GetRestaurantsInCityAdressesAsync(city, cancellationToken));
        }
    }
}
