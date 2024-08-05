using Agua.Service.EventHandler.Commands.Facturas;
using Agua.Service.Queries.DTOs.Facturas;
using Agua.Service.Queries.Queries.Facturas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agua.Service.EventHandler.Commands.CFDIs;
using System.IO;

namespace Agua.Api.Controllers.Facturas.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/Agua/cfdi")]
    public class FacturaCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacturaCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Consumes("multipart/form-data")]
        [Route("createFactura")]
        [HttpPost]
        public async Task<IActionResult> CreateFactura([FromForm] FacturaCreateCommand request)
        {
            var factura = await _mediator.Send(request);
            return Ok(factura);
        }

        [Consumes("multipart/form-data")]
        [Route("updateFactura")]
        [HttpPut]
        public async Task<IActionResult> UpdateFactura([FromForm] FacturaUpdateCommand request)
        {
            var factura = await _mediator.Send(request);
            return Ok(factura);
        }

        [Route("createHistorialMF")]
        [HttpPost]
        public async Task<IActionResult> CreateHistorialMF([FromBody] HistorialMFCreateCommand request)
        {
            var historial = await _mediator.Send(request);
            if (historial != null)
            {
                return Ok(historial);
            }
            return BadRequest();
        }
    }
}
