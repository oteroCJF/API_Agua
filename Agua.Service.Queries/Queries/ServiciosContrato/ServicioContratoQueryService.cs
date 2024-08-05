using Agua.Persistence.Database;
using Agua.Service.Queries.DTOs.ServiciosContratos;
using Agua.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agua.Service.Queries.Queries.ServiciosContrato
{
    public interface IServicioContratoQueryService
    {
        Task<List<ServicioContratoDto>> GetServicioContratoListAsync(int contrato);
    }

    public class ServicioContratoQueryService : IServicioContratoQueryService
    {
        private readonly ApplicationDbContext _context;

        public ServicioContratoQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServicioContratoDto>> GetServicioContratoListAsync(int contrato)
        {
            var servicios = await _context.ServicioContrato.Where(sc => sc.ContratoId == contrato).ToListAsync();

            return servicios.MapTo<List<ServicioContratoDto>>();
        }
    }
}
