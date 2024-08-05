using Agua.Service.EventHandler.Commands.EntregableContratacion;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Agua.Api.Controllers.EntregablesContratacion.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/agua/entregablesContratacion")]
    public class EContratacionCommandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EContratacionCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Consumes("multipart/form-data")]
        [Route("updateEntregableContratacion")]
        [HttpPut]
        public async Task<IActionResult> UpdateEntregable([FromForm] EntregableContratacionUpdateCommand entregable)
        {
            int status = await _mediator.Send(entregable);
            return Ok(status);
        }
    }
}
