using Microsoft.AspNetCore.Mvc;

namespace Investment.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentsController : ControllerBase
    {
        private readonly ILogger<InvestmentsController> _logger;

        public InvestmentsController(ILogger<InvestmentsController> logger)
        {
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<ActionResult> Create()
        {
            return Ok();
        }
    }
}