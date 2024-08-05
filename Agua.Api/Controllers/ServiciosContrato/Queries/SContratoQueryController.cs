using Agua.Service.EventHandler.Commands.ServiciosContrato;
using Agua.Service.Queries.DTOs.ServiciosContratos;
using Agua.Service.Queries.Queries.ServiciosContrato;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agua.Api.Controllers.ServiciosContrato.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/agua/servicioContrato")]
    public class SContratoQueryController : ControllerBase
    {
        private readonly IServicioContratoQueryService _scontrato;

        public SContratoQueryController(IServicioContratoQueryService scontrato)
        {
            _scontrato = scontrato;
        }

        [Route("getServiciosContrato/{contrato}")]
        [HttpGet]
        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            return await _scontrato.GetServicioContratoListAsync(contrato);
        }
    }
}
