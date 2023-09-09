using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Domain;
using Order.Service.EventHandler.Commands;
using Order.Service.Queries;
using Service.Common.Collection;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderQueryService _orderQueryService;
        private readonly IMediator _mediator;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderQueryService orderQueryService,
            IMediator mediator)
        {
            _logger = logger;
            _orderQueryService = orderQueryService;
            _mediator = mediator;
        }

        //orders
        [HttpGet]
        public async Task<DataCollection<OrderDto>> GetAll(int page = 1, int take = 10, string ids = null)
        {      
            return await _orderQueryService.GetAllAsync(page, take);
        }

        //orders/1
        [HttpGet("{id}")]
        public async Task<OrderDto> Get(int id)
        {
            return await _orderQueryService.GetAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateCommand command)
        {
            await _mediator.Publish(command);
            return Ok();
        }
    }
}
