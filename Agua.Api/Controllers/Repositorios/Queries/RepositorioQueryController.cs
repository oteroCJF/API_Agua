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

namespace Agua.Api.Controllers.Repositorios.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/Agua/repositorios")]
    public class RepositorioCommandController : ControllerBase
    {
        private readonly IRepositorioQueryService _repositorios;

        public RepositorioCommandController(IRepositorioQueryService repositorios)
        {
            _repositorios = repositorios;
        }

        [HttpGet("{anio}")]
        public async Task<List<RepositorioDto>> GetRepositorios(int anio)
        {
            return await _repositorios.GetAllRepositoriosAsync(anio);
        }

        [HttpGet("getRepositorioByAMC/{anio}/{mes}/{contrato}")]
        public async Task<RepositorioDto> GetRepositorioByAMC(int anio, int mes, int contrato)
        {
            return await _repositorios.GetRepositorioByAMCAsync(anio, mes, contrato);
        }
        
        [HttpGet("getRepositorioById/{id}")]
        public async Task<RepositorioDto> GetRepositorioById(int id)
        {
            return await _repositorios.GetRepositorioByIdAsync(id);
        }
    }
}
