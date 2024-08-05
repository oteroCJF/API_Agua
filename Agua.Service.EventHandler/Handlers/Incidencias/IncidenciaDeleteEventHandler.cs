using Agua.Persistence.Database;
using Agua.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Agua.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Data.SqlClient;

namespace Agua.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaDeleteEventHandler : IRequestHandler<IncidenciaDeleteCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaDeleteEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(IncidenciaDeleteCommand request, CancellationToken cancellationToken)
        {
            var incidencia = _context.Incidencias.SingleOrDefault(i => i.Id == request.Id);

            try
            {
                incidencia.FechaEliminacion = DateTime.Now;
                await _context.SaveChangesAsync();
                return incidencia.Id;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                return -1;
            }
        }
    }
}
