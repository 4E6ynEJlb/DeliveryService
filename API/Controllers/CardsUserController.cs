using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("cards")]
    [Authorize("RequireUserRole")]
    public class CardsUserController : ControllerBase
    {
        private readonly ICardsUserService _cardsUserService;
        public CardsUserController(ICardsUserService cardsUserService) 
        {
            _cardsUserService = cardsUserService;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetCard(string number, CancellationToken cancellationToken)
        {
            if (number == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            var cards = await _cardsUserService.GetUserCardsAsync(userGuid, cancellationToken);
            if (cards.Where(c => c.Number == number).Count() != 0)
                return Ok(await _cardsUserService.GetCardByNumberAsync(number, cancellationToken));
            return Forbid("You don\'t have a card with that number");
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            return Ok(await _cardsUserService.GetUserCardsAsync(userGuid, cancellationToken));
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Add(CardModel card, CancellationToken cancellationToken)
        {
            if (card == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _cardsUserService.AddCardAsync(card, userGuid, cancellationToken);
            return Ok();
        }
        [Route("[action]")]
        [HttpDelete]
        public async Task<IActionResult> Remove(string number, CancellationToken cancellationToken)
        {
            if (number == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            var cards = await _cardsUserService.GetUserCardsAsync(userGuid, cancellationToken);
            if (cards.Where(c => c.Number == number).Count() != 0)
            {
                await _cardsUserService.RemoveCardAsync(number, cancellationToken);
                return Ok();
            }
            return Forbid("You don\'t have a card with that number");
        }
    }
}
