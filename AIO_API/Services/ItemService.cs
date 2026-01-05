using AIO_API.Entities;
using AIO_API.Interfaces;
using AIO_API.Models.ItemDto;
using AutoMapper;

namespace AIO_API.Services
{
    public class ItemService : IItemService
    {
        private readonly AieDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemService> _logger;

        public ItemService(AieDbContext dbContext, IMapper mapper, ILogger<ItemService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<ItemDto> GetAll()
        {
            var items = _dbContext.Items.ToList();
            var result = _mapper.Map<List<ItemDto>>(items);
            return result;
        }
    }
}
