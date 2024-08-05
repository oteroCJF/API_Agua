using Agua.Persistence.Database;
using Agua.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Agua.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Agua.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaCreateEventHandler : IRequestHandler<IncidenciaCreateCommand, Incidencia>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incidencia> Handle(IncidenciaCreateCommand request, CancellationToken cancellationToken)
        {
            var incidencia = new Incidencia();
            try
            {
                incidencia = new Incidencia
                {
                    UsuarioId = request.UsuarioId,
                    IncidenciaId = request.IncidenciaId,
                    CedulaEvaluacionId = request.CedulaEvaluacionId,
                    Pregunta = request.Pregunta,
                    FechaProgramada = request.FechaProgramada,
                    FechaEntrega = request.FechaEntrega,
                    HoraProgramada = request.HoraProgramada,
                    HoraRealizada = request.HoraRealizada,
                    Cantidad = request.Cantidad,
                    Observaciones = request.Observaciones,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                };

                await _context.AddAsync(incidencia);
                await _context.SaveChangesAsync();

                return incidencia;
            }
            catch(Exception ex)
            {
                string message = ex.ToString();
                return null;
            }
        }
    }
}
