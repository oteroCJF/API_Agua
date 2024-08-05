using Agua.Persistence.Database;
using Agua.Service.EventHandler.Commands.Incidencias;
using Agua.Service.EventHandler.Commands.Respuestas;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Agua.Domain.DCedulaEvaluacion;

namespace Agua.Service.EventHandler.Handlers.Respuestas
{
    public class RespuestaUpdateEventHandler : IRequestHandler<RespuestasUpdateCommand, RespuestaEvaluacion>
    {
        private readonly ApplicationDbContext _context;

        public RespuestaUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RespuestaEvaluacion> Handle(RespuestasUpdateCommand respuestas, CancellationToken cancellationToken)
        {
            try
            {
                RespuestaEvaluacion respuesta = _context.Respuestas
                .Where(r => r.CedulaEvaluacionId == respuestas.CedulaEvaluacionId &&
                                r.Pregunta == respuestas.Pregunta)
                .FirstOrDefault();

                CedulaEvaluacion cedula = _context.CedulaEvaluacion.Single(c => c.Id == respuestas.CedulaEvaluacionId);

                respuesta.Respuesta = respuestas.Respuesta;
                respuesta.Detalles = respuestas.Detalles != null ? respuestas.Detalles : "";
                respuesta.FechaActualizacion = DateTime.Now;

                cedula.UsuarioId = respuestas.UsuarioId;

                await _context.SaveChangesAsync();

                return respuesta;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

    }
}
