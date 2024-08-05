using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Agua.Service.Queries.DTOs.CedulaEvaluacion;
using Agua.Service.EventHandler.Commands.Respuestas;
using MediatR;
using System;
using Agua.Service.Queries.Queries.Respuestas;

namespace Agua.Api.Controllers.Respuestas.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/Agua/respuestasEvaluacion")]
    public class RespuestaCommandController : ControllerBase
    {
        private readonly IRespuestasQueryService _respuestas;
        private readonly IMediator _mediator;

        public RespuestaCommandController(IRespuestasQueryService respuestas, IMediator mediator)
        {
            _respuestas = respuestas;
            _mediator = mediator;
        }

        [Route("updateRespuestasByCedula")]
        [HttpPut]
        public async Task<IActionResult> UpdateRespuestasByCedula([FromBody] List<RespuestasUpdateCommand> respuestas)
        {
            try
            {
                foreach (var rs in respuestas)
                {
                    await _mediator.Send(rs);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest();
            }

        }
    }
}
