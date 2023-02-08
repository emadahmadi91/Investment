using Investment.Application.Investments.Commands.CreateInvestmentItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Investment.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentsController : ControllerBase
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateInvestmentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}