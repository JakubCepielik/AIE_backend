using AIO_API.Interfaces;
using AIO_API.Models.ItemDto;
using AIO_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIO_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _ItemService;

        public ItemController(IItemService ItemService)
        {
            _ItemService = ItemService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ItemDto>> GetAll()
        {
            var items = _ItemService.GetAll();
            return Ok(items);
        }

    }
}
