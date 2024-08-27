using Application.Interfaces;
using Domain.Models.VievModels;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("goods")]
    [ApiController]
    public class GoodsUserController : ControllerBase
    {
        private readonly IGoodsUserService _goodsUserService;
        private readonly IFileClient _fileClient;
        public GoodsUserController(IGoodsUserService goodsUserService, IFileClient fileClient) 
        {
            _goodsUserService = goodsUserService;
            _fileClient = fileClient;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetProduct(int article, CancellationToken cancellationToken)
        {
            return Ok(await _goodsUserService.GetProductAsync(article, cancellationToken));
        }
        [HttpGet]
        [Route("[action]/{imagename}")]
        public async Task<IActionResult> GetImage(string imagename, CancellationToken cancellationToken)
        {
            if (imagename == null)
                return BadRequest("Arguments are null");
            var stream = await _fileClient.GetAsync(imagename, cancellationToken);
            return File(stream, "application/octet-stream", imagename);
            
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetHotList(int count, CancellationToken cancellationToken)
        {
            return Ok(await _goodsUserService.GetHotGoodsListAsync(count, cancellationToken));
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetList(int page, int pageSize, GoodsListOptionsModel listOptions, CancellationToken cancellationToken)
        {
            if (listOptions == null)
                return BadRequest("Arguments are null");
            return Ok(await _goodsUserService.GetVisibleGoodsArrayAsync(page, pageSize, listOptions, cancellationToken));
        }
        [HttpPatch]
        [Route("rate")]
        [Authorize("[action]")]
        public async Task<IActionResult> Rate(int article, int mark, CancellationToken cancellationToken)
        {
            await _goodsUserService.RateProductAsync(article, mark, cancellationToken);
            return Ok();
        }
    }
}
