using Agua.Service.EventHandler.Commands.LogCedulas;
using Agua.Service.Queries.DTOs.LogCedulas;
using Agua.Service.Queries.Queries.LogCedulas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agua.Service.Queries.Queries.LogOficios;
using Agua.Service.EventHandler.Commands.LogOficios;
using Agua.Service.Queries.DTOs.LogOficios;

namespace Agua.Api.Controllers.Historiales
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/Agua/logOficios")]
    public class LogOficioController : ControllerBase
    {
        private readonly ILogOficioQueryService _logs;
        private readonly IMediator _mediator;

        public LogOficioController(ILogOficioQueryService logs, IMediator mediator)
        {
            _logs = logs;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getHistorialByOficio/{oficio}")]
        public async Task<List<LogOficioDto>> GetHistorialByOficio(int oficio)
        {
            return await _logs.GetHistorialOficio(oficio);
        }

        [HttpPost]
        [Route("createHistorial")]
        public async Task<IActionResult> CreateHistorial([FromBody] LogOficiosCreateCommand historial)
        {
            await _mediator.Send(historial);
            return Ok();
        }
    }
}
