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

namespace Agua.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaUpdateEventHandler : IRequestHandler<IncidenciaUpdateCommand, Incidencia>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incidencia> Handle(IncidenciaUpdateCommand request, CancellationToken cancellationToken)
        {
            var incidencia = _context.Incidencias.SingleOrDefault(i => i.Id == request.Id);

            incidencia.UsuarioId = request.UsuarioId;
            incidencia.IncidenciaId = request.IncidenciaId;
            incidencia.CedulaEvaluacionId = request.CedulaEvaluacionId;
            incidencia.Pregunta = request.Pregunta;
            incidencia.FechaProgramada = request.FechaProgramada;
            incidencia.FechaEntrega = request.FechaEntrega;
            incidencia.HoraProgramada = request.HoraProgramada;
            incidencia.HoraRealizada = request.HoraRealizada;
            incidencia.Cantidad = request.Cantidad;
            incidencia.Observaciones = request.Observaciones;
            incidencia.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return incidencia;
        }
    }
}
