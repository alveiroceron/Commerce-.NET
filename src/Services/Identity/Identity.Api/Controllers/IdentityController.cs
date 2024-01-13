using Identity.Service.EventHandlers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("v1/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator; 
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateCommand command) 
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok();
            }
            return BadRequest();    
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Authentication(UserLoginCommand command)
        {
            if (ModelState.IsValid)
            { 
                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                {
                    return BadRequest("Acess denied.");
                }

                return Ok(result);
            }
            return BadRequest();
        }
    }
}
