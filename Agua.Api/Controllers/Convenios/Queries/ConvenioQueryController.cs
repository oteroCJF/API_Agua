using Agua.Service.Queries.Queries.Convenios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agua.Service.Queries.DTOs.Convenios;
using Agua.Service.Queries.DTOs.RubrosConvenios;

namespace Agua.Api.Controllers.Convenios.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/agua/convenios")]
    public class ConvenioQueryController : ControllerBase
    {
        private readonly IConvenioQueryService _convenio;

        public ConvenioQueryController(IConvenioQueryService convenio)
        {
            _convenio = convenio;
        }

        [HttpGet]
        [Route("getConveniosByContrato/{contrato}")]
        public async Task<List<ConvenioDto>> getConveniosByContrato(int contrato)
        {
            List<ConvenioDto> convenios = await _convenio.GetConveniosByContratoAsync(contrato);
            return convenios;
        }

        [HttpGet]
        [Route("getConvenioById/{convenio}")]
        public async Task<ConvenioDto> getConveniosById(int convenio)
        {
            ConvenioDto convenios = await _convenio.GetConvenioByIdAsync(convenio);
            return convenios;
        }

        [HttpGet]
        [Route("getRubrosByConvenio/{convenio}")]
        public async Task<List<RubroConvenioDto>> GetRubrosConvenio(int convenio)
        {
            List<RubroConvenioDto> convenios = await _convenio.GetRubrosConvenioAsync(convenio);
            return convenios;
        }
    }
}
