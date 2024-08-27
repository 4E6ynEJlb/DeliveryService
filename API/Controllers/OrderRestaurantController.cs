using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize("RequireRestaurantRole")]
    public class OrderRestaurantController : ControllerBase
    {
        private readonly IOrderRestaurantService _orderRestaurantService;
        public OrderRestaurantController(IOrderRestaurantService orderRestaurantService) 
        {
            _orderRestaurantService = orderRestaurantService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetOrder(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.GetOrderByIdAsync(id, cancellationToken);
            return Ok();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetList(int count, Coordinates restaurantCoordinates, CancellationToken cancellationToken)
        {
            if (restaurantCoordinates == null)
                return BadRequest("Arguments are null");
            return Ok(await _orderRestaurantService.GetOrdersListAsync(count, restaurantCoordinates, cancellationToken));
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> Accept(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.AcceptOrderAsync(id, cancellationToken);
            return Ok();
        }
        [HttpPatch]
        [Route("[action]")]
        public async Task<IActionResult> MarkAsCooked(Guid id, int article, bool wasCookedEarlier, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.RemoveUnitFromListAsync(id, article, wasCookedEarlier, cancellationToken);
            return Ok();
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> CloseOrder(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderRestaurantService.RemoveOrderAsync(id, cancellationToken);
            return Ok();
        }
    }
}
