using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Agua.Service.Queries.DTOs.Incidencias;
using Agua.Service.EventHandler.Commands.Incidencias;
using MediatR;
using Agua.Service.Queries.Queries.Incidencias;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Agua.Api.Controllers.Incidencias.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/Agua/incidenciasCedula")]
    public class IncidenciaCommandController : ControllerBase
    {
        private readonly IIncidenciasQueryService _incidenciasQuery;
        private readonly IMediator _mediator;

        public IncidenciaCommandController(IMediator mediator, IIncidenciasQueryService incidenciasQuery)
        {
            _incidenciasQuery = incidenciasQuery;
            _mediator = mediator;
        }

        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> InsertaIncidencia([FromBody] IncidenciaCreateCommand incidencia)
        {
            await _mediator.Send(incidencia);
            return Ok();
        }

        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> ActualizarIncidencia([FromBody] IncidenciaUpdateCommand incidencia)
        {
            await _mediator.Send(incidencia);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> EliminarIncidencias([FromBody] IncidenciaDeleteCommand incidencia)
        {
            var lIncidencias = await _incidenciasQuery.GetIncidenciasByPreguntaAndCedula(incidencia.CedulaEvaluacionId, incidencia.Pregunta);
            
            foreach (var inc in lIncidencias) 
            {
                incidencia.Id = inc.Id;
                await _mediator.Send(incidencia);
            }

            return Ok(lIncidencias.Count);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]
        public async Task<IActionResult> EliminarIncidencia([FromBody] IncidenciaDeleteCommand request)
        {
            var incidencia = await _mediator.Send(request);

            return Ok(incidencia);
        }
    }
}
