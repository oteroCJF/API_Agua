using Agua.Persistence.Database;
using Agua.Service.EventHandler.Commands.Incidencias;
using Agua.Service.EventHandler.Commands.LogCedulas;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Agua.Domain.DHistorial;

namespace Agua.Service.EventHandler.Handlers.LogCedulas
{
    public class LogCedulaCreateEventHandler : IRequestHandler<LogCedulasCreateCommand, LogCedula>
    {
        private readonly ApplicationDbContext _context;

        public LogCedulaCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LogCedula> Handle(LogCedulasCreateCommand logs, CancellationToken cancellationToken)
        {
            try
            {
                var log = new LogCedula
                {
                    CedulaEvaluacionId = logs.CedulaEvaluacionId,
                    UsuarioId = logs.UsuarioId,
                    EstatusId = logs.EstatusId,
                    Observaciones = logs.Observaciones,
                    FechaCreacion = DateTime.Now
                };

                await _context.AddAsync(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
