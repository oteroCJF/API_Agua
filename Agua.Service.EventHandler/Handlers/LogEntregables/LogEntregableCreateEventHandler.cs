using Agua.Persistence.Database;
using Agua.Service.EventHandler.Commands.LogEntregables;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Agua.Domain.DHistorialEntregables;

namespace Agua.Service.EventHandler.Handlers.LogEntregables
{
    public class LogEntregableCreateEventHandler : IRequestHandler<LogEntregablesCreateCommand, LogEntregable>
    {
        private readonly ApplicationDbContext _context;
        public LogEntregableCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LogEntregable> Handle(LogEntregablesCreateCommand logs, CancellationToken cancellationToken)
        {
            try 
            {
                var log = new LogEntregable
                {
                    CedulaEvaluacionId = logs.CedulaEvaluacionId,
                    UsuarioId = logs.UsuarioId,
                    EstatusId = logs.EstatusId,
                    EntregableId = logs.EntregableId,
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
