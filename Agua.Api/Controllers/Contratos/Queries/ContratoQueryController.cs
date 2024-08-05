using Agua.Service.Queries.DTOs.Contratos;
using Agua.Service.Queries.Queries.Contratos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agua.Api.Controllers.Contratos.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/agua/contratos")]
    public class ContratoQueryController : ControllerBase
    {
        private readonly IContratosQueryService _contratos;

        public ContratoQueryController(IContratosQueryService contratos)
        {
            _contratos = contratos;
        }

        [Route("getContratos")]
        [HttpGet]
        public async Task<List<ContratoDto>> GetAllContratos()
        {
            return await _contratos.GetAllContratosAsync();
        }

        [Route("getContratoById/{id}")]
        [HttpGet]
        public async Task<ContratoDto> GetContratoById(int id)
        {
            return await _contratos.GetContratoByIdAsync(id);
        }

    }
}
