using Customer.Service.EventHandlers.Commands;
using Customer.Service.Queries;
using Customer.Service.Queries.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Common.Collection;

namespace Customer.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientQueryService _clientQueryService;
        private readonly IMediator _mediator;
        public ClientController(
            IClientQueryService clientQueryService,
            IMediator mediator)
        {
            _clientQueryService = clientQueryService;
            _mediator = mediator;
        }

        //clients
        [HttpGet]
        public async Task<DataCollection<ClientDto>> GetAll(int page = 1, int take = 10, string ids = null)
        {
            IEnumerable<int> clients = null;

            if (!string.IsNullOrEmpty(ids))
            {
                clients = ids.Split(',').Select(x => Convert.ToInt32(x));
            }

            return await _clientQueryService.GetAllAsync(page, take, clients);
        }

        //clients/1
        [HttpGet("{id}")]
        public async Task<ClientDto> Get(int id)
        { 
            return await _clientQueryService.GetAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientCreateCommand command)
        { 
            await _mediator.Publish(command);
            return Ok();
        }
    }
}
