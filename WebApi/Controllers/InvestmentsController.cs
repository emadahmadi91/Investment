using Investment.Application.Investments.Commands.CreateInvestmentItem;
using Investment.Application.Investments.Commands.DeleteInvestmentItem;
using Investment.Application.Investments.Commands.UpdateInvestment;
using Investment.Application.Investments.Query;
using Investment.Domain.Dto;
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
        
        [HttpGet]
        public async Task<ActionResult<List<InvestmentDto>>> Get()
        {
            return await Mediator.Send(new GetInvestmentsQuery());
        }
        
        [HttpDelete("{name}")]
        public async Task<ActionResult> Delete(string name)
        {
            await Mediator.Send(new DeleteTodoItemCommand(name));

            return NoContent();
        }
        
        
        [HttpPut("{name}")]
        public async Task<ActionResult> UpdateItemDetails(string name,
            [FromBody] UpdateInvestmentDTO updateInvestmentDto)
        {
            var updateInvestmentCommand = new UpdateInvestmentCommand
                { OldName = name, UpdateInvestmentDto = updateInvestmentDto };
            await Mediator.Send(updateInvestmentCommand);

            return NoContent();
        }
    }
}