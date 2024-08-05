using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Agua.Service.Queries.DTOs.Facturas;
using Agua.Service.EventHandler.Commands.Repositorios;
using Agua.Service.Queries.Queries.Repositorios;

namespace Agua.Api.Controllers.Repositorios.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/Agua/repositorios")]
    public class RepositorioCommandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RepositorioCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("createRepositorio")]
        [HttpPost]
        public async Task<IActionResult> CreateRepositorio([FromBody] RepositorioCreateCommand Repositorio)
        {
            int status = await _mediator.Send(Repositorio);
            return Ok(status);
        }

        [Route("updateRepositorio")]
        [HttpPut]
        public async Task<IActionResult> UpdateRepositorio([FromBody] RepositorioUpdateCommand Repositorio)
        {
            var status = await _mediator.Send(Repositorio);
            return Ok(status);
        }
    }
}
