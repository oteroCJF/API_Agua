using Agua.Service.Queries.DTOs.CedulaEvaluacion;
using Agua.Service.Queries.Queries.Incidencias;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agua.Service.Queries.Queries.CedulasEvaluacion;
using Agua.Service.EventHandler.Commands.CedulasEvaluacion;
using Agua.Service.EventHandler.Commands.LogCedulas;
using Agua.Service.EventHandler.Commands.CedulasEvaluacion.DBloquearCedula;
using Agua.Service.EventHandler.Commands.CedulasEvaluacion.ActualizacionCedula;
using Agua.Service.Queries.Queries.Respuestas;

namespace Agua.Api.Controllers.CedulasEvaluacion.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/Agua/cedulaEvaluacion")]
    public class AguaCommandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AguaCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("enviarCedula")]
        [HttpPut]
        public async Task<IActionResult> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand request)
        {
            var cedula = await _mediator.Send(request);

            if (cedula != null)
            {
                var log = new LogCedulasCreateCommand
                {
                    UsuarioId = request.UsuarioId,
                    CedulaEvaluacionId = cedula.Id,
                    EstatusId = request.EstatusId,
                    Observaciones = request.Observaciones
                };

                var logs = await _mediator.Send(log);

                if (logs != null)
                {
                    return Ok(cedula);
                }
            }
            return Ok(cedula);
        }

        [Route("dbloquearCedula")]
        [HttpPut]
        public async Task<IActionResult> DBloquearCedula([FromBody] DBloquearCedulaUpdateCommand request)
        {
            var cedula = await _mediator.Send(request);

            if (cedula != null)
            {
                var log = new LogCedulasCreateCommand
                {
                    UsuarioId = request.UsuarioId,
                    CedulaEvaluacionId = cedula.Id,
                    EstatusId = cedula.EstatusId,
                    Observaciones = request.Observaciones
                };

                var logs = await _mediator.Send(log);

                if (logs != null)
                {
                    return Ok(cedula);
                }
            }
            return Ok(cedula);
        }

        [Route("updateCedula")]
        [HttpPut]
        public async Task<IActionResult> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand request)
        {
            var cedula = await _mediator.Send(request);

            if (cedula != null)
            {
                var log = new LogCedulasCreateCommand
                {
                    UsuarioId = request.UsuarioId,
                    CedulaEvaluacionId = cedula.Id,
                    EstatusId = cedula.EstatusId,
                    Observaciones = request.Observaciones
                };

                var logs = await _mediator.Send(log);

                if (logs != null)
                {
                    return Ok(cedula);
                }
            }
            return Ok(cedula);
        }
    }
}
