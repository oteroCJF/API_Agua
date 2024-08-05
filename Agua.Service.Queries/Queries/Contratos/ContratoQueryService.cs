using Agua.Persistence.Database;
using Agua.Service.Queries.DTOs.Contratos;
using Agua.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agua.Service.Queries.Queries.Contratos
{
    public interface IContratosQueryService
    {
        Task<List<ContratoDto>> GetAllContratosAsync();
        Task<ContratoDto> GetContratoByIdAsync(int id);
    }

    public class ContratoQueryService : IContratosQueryService
    {
        private readonly ApplicationDbContext _context;

        public ContratoQueryService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContratoDto>> GetAllContratosAsync()
        {
            var collection = await _context.Contratos.OrderBy(x => x.Id).ToListAsync();

            return collection.MapTo<List<ContratoDto>>();
        }

        public async Task<ContratoDto> GetContratoByIdAsync(int id)
        {
            var contrato = await _context.Contratos.SingleOrDefaultAsync(x => x.Id == id);
            return contrato != null ? contrato.MapTo<ContratoDto>() : new ContratoDto();
        }
    }
}
