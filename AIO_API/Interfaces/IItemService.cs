using AIO_API.Models.CharacterDto;
using AIO_API.Models.ItemDto;

namespace AIO_API.Interfaces
{
    public interface IItemService
    {
        public IEnumerable<ItemDto> GetAll();
    }
}
