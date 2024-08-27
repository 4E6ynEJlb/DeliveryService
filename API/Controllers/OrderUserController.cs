using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize("RequireUserRole")]
    public class OrderUserController : ControllerBase
    {
        private readonly IOrderUserService _orderUserService;
        public OrderUserController(IOrderUserService orderUserService) 
        {
            _orderUserService = orderUserService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetOrder(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            return Ok(await _orderUserService.GetOrderByIdAsync(id, cancellationToken));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetUserOrders(CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            return Ok(await _orderUserService.GetOrdersByUserIdAsync(userGuid, cancellationToken));
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> MakeOrder(OrderCreateModel orderCreateModel, CancellationToken cancellationToken)
        {
            if (orderCreateModel == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            OrderModel orderModel = orderCreateModel.ToOrderModel(userGuid);
            await _orderUserService.AddOrderAsync(orderModel, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> CancelOrder(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _orderUserService.RemoveOrderAsync(id, cancellationToken);
            return Ok();
        }
    }
}
